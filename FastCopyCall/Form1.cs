using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Diagnostics;

using Codeplex.Data;
using NFsCGI;


namespace FastCopyCall
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private FastCopyOpt ToOpt(string s)
		{
			FastCopyOpt fco = new FastCopyOpt();
			fco.FromJson(s);
			return fco;

		}
		private void Form1_Load(object sender, EventArgs e)
		{
			string ExeName = Path.Combine( Path.GetDirectoryName( Application.ExecutablePath),"FastCopy.exe");
			string[] cmds = System.Environment.GetCommandLineArgs();
			//%1%Qsrc%Q%C%QE%C%Y%Ypool2%Y%Y%Q%K%Qdst%Q%C%Q%Y%Y%Y%Y192.168.10.88%Y%Ysv04%Y%Y%Q%K%Qargs%Q%C%Q%S%Y%Lcmd=sync%S%Y%Lforce_close%S%Y%Lno_confirm_del%S%Y%Lopen_window%Q%K%Qcaption%Q%C%Q001%Q%2
			FastCopyOpt fco = new FastCopyOpt();
			if (cmds.Length>1)
			{
				fco.FromJsonD(cmds[1]);
            }
            else
            {
				string ds = "%1%Qsrc%Q%C%QE%C%Y%Ypool2%Y%Y%Q%K%Qdst%Q%C%Q%Y%Y%Y%Y192.168.10.88%Y%Ysv04%Y%Y%Q%K%Qargs%Q%C%Q%S%Y%Lcmd=sync%S%Y%Lforce_close%S%Y%Lno_confirm_del%S%Y%Lopen_window%Q%K%Qcaption%Q%C%Q001%Q%2";
				fco.FromJsonD(ds);
			}

			this.Text = fco.ToJson();
			textBox1.Text = fco.ToJson();

			Process p = new Process();
			p.StartInfo.FileName = ExeName;
			p.StartInfo.Arguments = fco.Options();
			p.SynchronizingObject = this;
			p.Exited += new EventHandler(p_Exited);
			p.EnableRaisingEvents = true;

			if (p.Start() == true)
			{
				textBox1.Text += "\r\n実行中\r\n";
			}
			else
			{
				textBox1.Text += "\r\n実行失敗\r\n" + ExeName;
#if DEBUG
#else
				Application.Exit();
#endif
			}
			//FastCopy.Exec
		}
		private void p_Exited(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
