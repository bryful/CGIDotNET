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
using System.Web;

using Codeplex.Data; // DynamicJson はこれ。
using System.Diagnostics; // Trace とかこれ。

using NFsCGI;

namespace FindFolder
{
	class Program
	{

		static string LoadPref(string p)
		{
			string ret = "";

			if (File.Exists(p) == false) return ret;
			try
			{
				string js = File.ReadAllText(p, Encoding.GetEncoding("utf-8"));
				dynamic obj = DynamicJson.Parse(js);
				DynamicJson dobj = (DynamicJson)obj;
				if(dobj.IsDefined("Path")==true)
				{
					ret = obj["Path"];
					if (Directory.Exists(ret) == false)
					{
						ret = "dir exists";
					}
				}
			}
			catch
			{
				ret = "";
			}

			return ret;
		}
		static string [] LoadTargetDir(string p)
		{
			string [] ret = new string[0];

			if (File.Exists(p) == false) return ret;
			try
			{
				string js = File.ReadAllText(p, Encoding.GetEncoding("utf-8"));
				dynamic obj = DynamicJson.Parse(js);
				DynamicJson dobj = (DynamicJson)obj;
				if (dobj.IsDefined("TargetDir") == true)
				{
					ret = obj["TargetDir"];
				}
			}
			catch
			{
				ret = new string[0];
			}

			return ret;
		}
		static void Main(string[] args)
		{
			// NFsCgiでpost/getの処理を行う
			NFsCgi cgi = new NFsCgi(args);
			/// lock処理
			if (cgi.CheckLockFile("FindFolder") == false)
			{
				cgi.WriteErr("<b>lock err<b>");
				return;
			}
			// CGIパラメータを獲得

			// 検索するフォルダ名
			string TargetName = "";
			cgi.Data.FindValueFromTag("TargetName", out TargetName);
			// 動作モード find listup etc
			string Mode = "";
			cgi.Data.FindValueFromTag("Mode", out Mode);
			// 対象ディレクトリのインデックス文字列
			string TargetIndexStr = "";
			cgi.Data.FindValueFromTag("TargetIndex", out TargetIndexStr);
			// 対象ディレクトリのインデックス
			int TargetIndexValue = 0;
			if (int.TryParse(TargetIndexStr, out TargetIndexValue)==false)
			{
				TargetIndexValue = 0;
			}
			// FormのSelect 項目
			string SelectItemStr = "";
			// 対象ディレクトリの知ると
			string[] TargetDirs = new string[0];

			//prefファイルを読み込む
			//prefファイルにはターゲットリストがあるフォルダパスが入っている
			string prmD = LoadPref(".pref");
			if (prmD == "")
			{
				cgi.WriteErr("<b>.pref err<b>");
				cgi.CloseLockFile();
				return;
			}
			else
			{
				//ターゲットリストを読み込む
				TargetDirs = LoadTargetDir(Path.Combine(prmD, "TargetDir.json"));
				if(TargetDirs.Length<=0)
				{
					cgi.WriteErr("<b>TargetDir.json err<b>");
					cgi.CloseLockFile();
					return;

				}
				else
				{
					// ターゲットリストからSelect項目のhtmlを作成。
					for (int i=0; i< TargetDirs.Length;i++)
					{
						string b = "<option {0} value=\"{1}\">{2}</option>\r\n";
						string ss = "";
						if (TargetIndexValue == i) ss = "selected";
						SelectItemStr += String.Format(b, ss, i, Path.GetFileName(TargetDirs[i]));
					}
				}
			}
			if (TargetIndexValue < 0) TargetIndexValue = 0;
			else if (TargetIndexValue > TargetDirs.Length - 1) TargetIndexValue = TargetDirs.Length - 1;


			// htmlの処理
			string htmlPath = Path.Combine(prmD, "base.html");
			NFsHtml html = new NFsHtml(htmlPath);

			//基本htmlがなかったら作成
			if(html.Html=="")
			{
				html.Html = Properties.Resources.baseHtml;
				html.Save(htmlPath);
			}
			// htmlの形成
			html.ReplaceTag("$TargetName", TargetName);
			html.ReplaceTag("$PATH_INFO", cgi.PATH_INFO);
			html.ReplaceTag("$QUERY_STRING", cgi.QUERY_STRING);
#if DEBUG
			html.ReplaceTag("$Data", cgi.Data.ToJson());
#endif
			html.ReplaceTag("$SelectItems", SelectItemStr);

			// フォルダ検索
			FindFolder ff = new FindFolder();

			string TargetPath = TargetDirs[TargetIndexValue];


			ff.SetTargetDir(TargetPath);

			//html.ReplaceTag("$TargetDir", ff.TargetDir);

			//対象ディレクトリがなかったらエラー TargetDir
			if (ff.TargetDir=="")
			{
				html.output();
				cgi.CloseLockFile();
				return;
			}
			// Modeがlistuoならデータベースを再構築
			if (Mode == "listup")
			{
				ff.ListupFolders(ff.TargetDir);
			}

			if (Mode == "find")
			{
				// フォルダ検索の実際の処理
				string[] sa = ff.Find(TargetName);//FolderPath

				string dlist = "";

				// 結果をhtmlへ
				if (sa.Length > 0)
				{
					foreach (string ss in sa)
					{
						dlist += "<li>" + ss + "</li>\r\n";
					}
				}
				else
				{
					dlist += "<li>None</li>\r\n";
				}
				dlist = "<ul class=\"big\">\r\n" + dlist + "</ul>\r\n";

				html.ReplaceTag("$FolderPath", dlist);
			}
			else
			{
				html.ReplaceTag("$FolderPath", "");
			}
			//出力
			html.output();

			// lock解除
			cgi.CloseLockFile();
		}
	}
}
