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
		static string targetDir = @" \\192.168.10.88\sv04\pool\sec\comic";
		static void Main(string[] args)
		{
			NFsCgi cgi = new NFsCgi(args);
			/// lock処理
			if (cgi.CheckLockFile("FindFolder") == false)
			{
				cgi.WriteErr();
				return;
			}
			string TargetName = "";
			cgi.Data.FindValueFromTag("TargetName", out TargetName);
			string Mode = "";
			cgi.Data.FindValueFromTag("Mode", out Mode);
			// htmlの処理
			NFsHtml html = new NFsHtml("./body.html");

			if(html.Html=="")
			{
				html.Html = Properties.Resources.baseHtml;
				html.Save("./body.html");
			}

			html.ReplaceTag("$TargetName", TargetName);
			html.ReplaceTag("$PATH_INFO", cgi.PATH_INFO);
			html.ReplaceTag("$QUERY_STRING", cgi.QUERY_STRING);
			html.ReplaceTag("$Data", cgi.Data.ToJson());

			FindFolder ff = new FindFolder(targetDir);

			if(ff.TargetDir=="")
			{
				html.output();
				return;
			}
			if (Mode == "listup")
			{
				//ff.ListupFolders(targetDir);
			}

			string[] sa = ff.Find(TargetName);//FolderPath

			string dlist = "";

			if(sa.Length>0)
			{
				foreach(string ss in sa)
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

			html.output();

			// lock解除
			cgi.CloseLockFile();
		}
	}
}
