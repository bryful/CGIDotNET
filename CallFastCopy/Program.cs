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

namespace CallFastCopy
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
				if (dobj.IsDefined("Path") == true)
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
		static FastCopyOptList LoadFastCopy(string p)
		{
			FastCopyOptList ret = new FastCopyOptList();
			ret.Load(p);
			return ret;
		}
		static void Main(string[] args)
		{
			// NFsCgiでpost/getの処理を行う
			NFsCgi cgi = new NFsCgi(args);
			/// lock処理
			if (cgi.CheckLockFile("fastcopy") == false)
			{
				cgi.WriteErr("<b>lock err<b>");
				return;
			}
			string pramD = LoadPref(".pref");
			if ((pramD=="")||(pramD== "dir exists"))
			{
				cgi.WriteErr("<b>pref err<b>");
				cgi.CloseLockFile();
				return;

			}



			// CGIパラメータを獲得
			// 動作モード find listup etc
			string Mode = "";
			cgi.Data.FindValueFromTag("Mode", out Mode);
			// 対象ディレクトリのインデックス文字列
			string TargetIndexStr = "";
			cgi.Data.FindValueFromTag("TargetIndex", out TargetIndexStr);
			// 対象ディレクトリのインデックス
			int TargetIndexValue = 0;
			if (int.TryParse(TargetIndexStr, out TargetIndexValue) == false)
			{
				TargetIndexValue = 0;
			}
			// FormのSelect 項目
			string SelectItemStr = "";
			// 対象ディレクトリの知ると
			string[] TargetDirs = new string[0];

			//prefファイルを読み込む
			//prefファイルにはターゲットリストがあるフォルダパスが入っている
			FastCopyOptList optlist = LoadFastCopy(Path.Combine(pramD, "fastcopy.json"));

			if (optlist.Items.Count<=0)
			{
				cgi.WriteErr("<b>fastcopy.json err<b>");
				cgi.CloseLockFile();
				return;
			}
			else
			{
				// ターゲットリストからSelect項目のhtmlを作成。
				for (int i = 0; i < optlist.Items.Count; i++)
				{
					string b = "<option {0} value=\"{1}\">{2}</option>\r\n";
					string ss = "";
					if (TargetIndexValue == i) ss = "selected";
					SelectItemStr += String.Format(b, ss, i, optlist.Items[i].caption);
				}
				
			}
			if (TargetIndexValue < 0) TargetIndexValue = 0;
			else if (TargetIndexValue > optlist.Items.Count - 1) TargetIndexValue = optlist.Items.Count - 1;


			// htmlの処理
			string htmlPath = Path.Combine(pramD, "base.html");
			NFsHtml html = new NFsHtml(htmlPath);

			//基本htmlがなかったら作成
			if (html.Html == "")
			{
				html.Html = Properties.Resources.baseHtml;
				html.Save(htmlPath);
			}
			// htmlの形成
			html.ReplaceTag("$PATH_INFO", cgi.PATH_INFO);
			html.ReplaceTag("$QUERY_STRING", cgi.QUERY_STRING);
#if DEBUG
			html.ReplaceTag("$Data", cgi.Data.ToJson());
#endif
			html.ReplaceTag("$SelectItems", SelectItemStr);



			if(Mode =="exec")
			{

			}


			Process[] fxs = FastCopy.ListupFastCopy();
			string FastCopyNow = "";
			if(fxs.Length>0)
			{
				for (int i=0; i< fxs.Length;i++)
				{
					string t = fxs[i].MainWindowTitle;
					FastCopyOpt fco = new FastCopyOpt();
					fco.FromJson(t);
					string ss = "<li>FastCopy src:[{0}] dst:[{1}] start:{2}</li>\r\n";
					FastCopyNow += String.Format(ss, fco.src,fco.dst, fxs[i].StartTime.ToString());
				}
				FastCopyNow = "<ul>\r\n" + FastCopyNow + "</ul>\r\n";
			}
			else
			{
				FastCopyNow = "起動なし";
			}
			html.ReplaceTag("$FastCopyNow", FastCopyNow);
			//出力
			html.output();

			// lock解除
			cgi.CloseLockFile();
		}
	}
}
