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

namespace FindFolder
{
	class Program
	{
		static string targetDirDB = @" \\192.168.10.88\sv04\pool\sec\comic\FindFolder.json";

		static string [] targetDirs = new string[]{
		@" \\192.168.10.88\sv04\pool\sec\comic\00",
		@" \\192.168.10.88\sv04\pool\sec\comic\10",
		@" \\192.168.10.88\sv04\pool\sec\comic\20",
		@" \\192.168.10.88\sv04\pool\sec\comic\30",
		@" \\192.168.10.88\sv04\pool\sec\comic\40",
		@" \\192.168.10.88\sv04\pool\sec\comic\99"
		};

		static void FindFolderName(ref List<DirectoryInfo> lst, string TargetName)
		{

			foreach(string s in targetDirs)
			{
				DirectoryInfo d1 = new DirectoryInfo(s);
				if (d1.Exists == false) continue;
				DirectoryInfo[] d2 = d1.GetDirectories("*" + TargetName + "*");
				if (d2.Length>0)
				{
					foreach(DirectoryInfo d3 in d2)
					{
						lst.Add(d3);
					}
				}

			}
		}
		static bool FindFolderNameDB(ref List<DirectoryInfo> lst, string TargetName)
		{
			bool ret = false;

			if (File.Exists(targetDirDB) == false) return ret;

			string pad = Path.GetDirectoryName(targetDirDB);

			string js = File.ReadAllText(targetDirDB, Encoding.GetEncoding("utf-8"));

			var json = DynamicJson.Parse(js);

			if ( json.IsDefined("Data") ==true)
			{
				string[] sa = json.Data;
				if(sa.Length>0)
				{
					int cnt = 0;
					string tn = TargetName.ToLower();
					foreach(string s in sa)
					{
						if( s.ToLower().IndexOf(tn)>=0)
						{
							lst.Add( new DirectoryInfo(Path.Combine(pad,s))  );
							cnt++;
						}
					}
					ret = (cnt > 0);
				}

			}

			return ret;

		}
		static void Main(string[] args)
		{

			CgiUtil cu = new CgiUtil(args);
			string TargetName = "";

			string dirList = "";

			if (cu.CheckLockFile("FindFolder") == false)
            {
				cu.WriteErr();
				return;
            }

			if (cu.Data.FindValueFromTag("folder",out TargetName))
            {
				List<DirectoryInfo> lst = new List<DirectoryInfo>();
				
				if (FindFolderNameDB(ref lst, TargetName) ==false)
				{
					FindFolderName(ref lst, TargetName);
				}
				if (lst.Count > 0)
				{
					foreach (DirectoryInfo d in lst)
					{
						dirList += "<li>" + d.Parent.Name + "/" + d.Name + "</li>\r\n";
					}
				}
				else
				{
					dirList += "<li>None</li>\r\n";
				}
			}
			dirList = "<ul class=\"big\">\r\n" + dirList + "</ul>\r\n";
			
			FileInfo fi = new FileInfo(".\\body.html");

			string html = "$FolderPath";
			if (fi.Exists == true)
			{
				html = File.ReadAllText(fi.FullName, Encoding.GetEncoding("utf-8"));
            }
            else
            {
				html = Properties.Resources.baseHtml;

			}
			html = html.Replace("$FolderPath", dirList);
			html = html.Replace("$TargetName", TargetName);
			//html = html.Replace("$QUERY_STRING", "QUERY_STRING:" + cu.QUERY_STRING);
			//html = html.Replace("$PATH_INFO", "PATH_INFO:" + cu.PATH_INFO);

			cu.WriteHtml(html);

			cu.CloseLockFile();
		}
	}
}
