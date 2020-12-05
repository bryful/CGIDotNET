using BRY;
using Codeplex.Data;
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

namespace FindFolderDB
{
	public class FindFolderDataBase
	{
		// *************************************************************
		private ListBox m_ListBox = null;
		public ListBox ListBox
		{
			get { return m_ListBox; }
			set
			{
				m_ListBox = value;
				if(m_ListBox!=null)
				{
					m_ListBox.Items.Clear();
				}
			}
		}
		// *************************************************************
		private string m_TargetDir = "";
		private List<string> m_Dirs = new List<string>();
		// *************************************************************
		public FindFolderDataBase()
		{
			Clear();
		}
		// *************************************************************
		public void Clear()
		{
			m_TargetDir = "";
			m_Dirs.Clear();
			if(m_ListBox!=null)
			{
				m_ListBox.Items.Clear();
			}

		}
		// *************************************************************
		private void FindFoders(DirectoryInfo diTop, int depth)
		{
			if (diTop.Exists == false) return;
			if (depth < 0) return;

			foreach (var di in diTop.EnumerateDirectories("*"))
			{
				string s = di.FullName;
				s = s.Substring(m_TargetDir.Length + 1);

				m_Dirs.Add(s);
				if (m_ListBox != null)
				{
					m_ListBox.Items.Add(s);
					m_ListBox.Refresh();
				}
			}
			if (depth >= 1)
			{
				foreach (var di in diTop.EnumerateDirectories("*"))
				{
					FindFoders(new DirectoryInfo(di.FullName), depth - 1);
				}
			}

		}
		// *************************************************************
		public void SetTargetDir(string p)
		{
			m_Dirs.Clear();
			DirectoryInfo di = new DirectoryInfo(p);
			if (di.Exists == false) return;
			if(m_ListBox!=null)
			{
				m_ListBox.Items.Clear();
			}
			m_TargetDir = di.FullName;
			FindFoders(di, 2);
		}
		// *************************************************************
		public string ToJson()
		{
			string ret = "";
			if (m_Dirs.Count <= 0) return ret;

			dynamic obj = new DynamicJson();

			obj["Count"] = (double)m_Dirs.Count;

			string[] sa = m_Dirs.ToArray();
			obj["Dirs"] = sa;

			ret = ((DynamicJson)obj).ToString();
			return ret;
		}
		// *************************************************************
		public bool ExportDB()
		{
			bool ret = false;

			if (m_Dirs.Count <= 0) return ret;
			string js = ToJson();

			string p = Path.Combine(m_TargetDir, "FindFolder.json");
			try
			{
				if (File.Exists(p))
				{
					File.Delete(p);
				}
				File.WriteAllText(p, js, Encoding.GetEncoding("utf-8"));
				ret = File.Exists(p);
			}
			catch
			{
				ret = false;
			}
			return ret;

		}
		// *************************************************************
	}
}
