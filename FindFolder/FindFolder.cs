using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Codeplex.Data; // DynamicJson はこれ。
using System.Diagnostics; // Trace とかこれ。

namespace FindFolder
{
	public class FindFolder
	{
		// ***************************************************************
		public readonly string TargetDirDBName = "FindFolder.json";
		private string m_TargetDir = "";
		public string TargetDir
		{
			get { return m_TargetDir; }
			set
			{
				SetTargetDir(value);
			}
		}
		// ***************************************************************
		private string [] m_Dirs = new string[0];
		// ***************************************************************
		/// <summary>
		/// クラスを作るだけ
		/// </summary>
		public FindFolder()
		{
			m_TargetDir = "";
		}
		// ***************************************************************
		/// <summary>
		/// ターゲットディレクトリを指定
		/// TargetDirが設定されていたら成功。フォルダリストも作成済み
		/// </summary>
		/// <param name="p">ターゲットディレクトリのフルパス</param>
		public FindFolder(string p)
		{
			SetTargetDir(p);
		}
		// ***************************************************************
		public string[] Find(string nm)
		{
			nm = nm.Trim();
			if (nm=="") return new string[0];
			List<string> ret = new List<string>();
			if (m_Dirs.Length <= 0) return new string[0];

			string nm2 = nm.ToLower();
			foreach(string s in m_Dirs)
			{
				string s2 = s.ToLower();
				if(s2.IndexOf(nm2)>=0)
				{
					ret.Add(s);
				}
			}

			return ret.ToArray();
		}
		// ***************************************************************
		/// <summary>
		/// ターゲットのディレクトリを設定
		/// 成功したらディレクトリリストを作成
		/// ディレクトリリストデータベースがあればそれを読み出す。
		/// </summary>
		/// <param name="p">ディレクトリの封rパス</param>
		/// <returns></returns>
		public bool SetTargetDir(string p)
		{
			bool ret = false;
			m_TargetDir = "";
			m_Dirs = new string[0];

			if (Directory.Exists(p) == false) return ret;

			//データベースがあるか調べる
			string db = Path.Combine(p, TargetDirDBName);
			bool ex = false;
			if (File.Exists(db) == true)
			{
				ex = ImportFindFoldersDB(db);
			}
			if(ex==false)
			{
				ex = ListupFolders(p);
				if(ex)
				{
					ExportFindFoldersDB();
				}
			}
			m_TargetDir = p;

			return ret;
		}
		// ***************************************************************
		/// <summary>
		/// ディレクトリリストを読み出す
		/// </summary>
		/// <param name="p">読み出すフォルダパス</param>
		/// <returns></returns>
		public bool ListupFolders(string p)
		{
			bool ret = false;
			m_Dirs = new string[0];
			m_TargetDir = "";
			if (p == "") return ret;
			DirectoryInfo di = new DirectoryInfo(p);
			if (di.Exists == false) return ret;
			List<string> dl = new List<string>();

			ListupFolders(ref dl, di, 2,p);
			if (dl.Count > 0)
			{
				m_Dirs = dl.ToArray();
				m_TargetDir = p;
				ret = true;
			}
			return ret;

		}
		// ***************************************************************
		/// <summary>
		/// ディレクトリリストを読み出す（再起用）
		/// </summary>
		/// <param name="dl">追加するディレクトリのリスト</param>
		/// <param name="diTop">読み出すディレクトリフルパス</param>
		/// <param name="depth">再起用のパラメータ</param>
		/// <param name="bs">読み出すディレクトリの基底パス</param>
		private void ListupFolders(ref List<string> dl, DirectoryInfo diTop, int depth,string bs)
		{
			if (depth <= 0) return;
			if (diTop.Exists == false) return;
			List<string> dirs = new List<string>();

			int ln = bs.Length;
			foreach (var di in diTop.EnumerateDirectories("*"))
			{
				dirs.Add(di.FullName);
			}
			if(dirs.Count>0)
			{
				foreach(string s in dirs)
				{
					string s2 = s.Substring(ln);
					dl.Add(s2);
				}
				if (depth >= 1)
				{
					foreach (string s in dirs)
					{
						ListupFolders(ref dl, new DirectoryInfo(s), depth - 1,bs);
					}
				}

			}
		}
		// ***************************************************************
		/// <summary>
		/// ディレクトリデータベースを書き出す
		/// </summary>
		/// <returns></returns>
		public bool ExportFindFoldersDB()
		{
			bool ret = false;
			if (m_Dirs.Length <= 0) return ret;
			if (m_TargetDir == "") return ret;

			dynamic obj = new DynamicJson();
			obj["Count"] = (double)m_Dirs.Length;
			obj["Dirs"] = m_Dirs;

			string s = ((DynamicJson)obj).ToString();

			string p = Path.Combine(m_TargetDir, TargetDirDBName);
			try
			{
				if (File.Exists(p))
				{
					File.Delete(p);
				}
				File.WriteAllText(p, s, Encoding.GetEncoding("utf-8"));
				ret = File.Exists(p);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		// ***************************************************************
		/// <summary>
		/// ディレクトリデータベースを書き出す
		/// </summary>
		/// <param name="p">データベースファイルのフルパス</param>
		/// <returns></returns>
		public bool ImportFindFoldersDB(string p)
		{
			bool ret = false;
			if (File.Exists(p) == false) return ret;
			string pad = Path.GetDirectoryName(p);
			string js = File.ReadAllText(p, Encoding.GetEncoding("utf-8"));
			var json = DynamicJson.Parse(js);

			if (json.IsDefined("Data") == true)
			{
				string[] sa = json.Data;
				if (sa.Length > 0)
				{
					m_Dirs = sa;
					m_TargetDir = pad;
					ret = true;
					return ret;
				}

			}

			return ret;
		}
		// ***************************************************************
		public string [] LoadDef(string p)
		{
			string [] ret = new string[0];

			if (File.Exists(p) == true)
			{
				try
				{
					string js = File.ReadAllText(p, Encoding.GetEncoding("utf-8"));

					dynamic obj = DynamicJson.Parse(js);

					DynamicJson dobj = (DynamicJson)obj;
					if( dobj.IsDefined("TargetDir")==true)
					{
						if (dobj.IsArray)
						{
							ret = (string[])obj["TargetDir"];
						}
						else
						{
							ret = new string[1];
							ret[0] = (string)obj["TargetDir"];
						}
					}
				}
				catch
				{
					ret = new string[0];
				}
			}
			return ret;
		}
		// ***************************************************************
		public bool SaveDef(string p,string [] sa)
		{
			bool ret = false;

			try
			{
				dynamic obj = new DynamicJson();
				obj["TargetDir"] = sa;

				string js = ((DynamicJson)obj).ToString();
				File.WriteAllText(p, js, Encoding.GetEncoding("utf-8"));
				ret = File.Exists(p);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
	}
}
