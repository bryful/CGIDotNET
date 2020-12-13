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


using NFsCGI;

namespace ExeTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			listBox1.Items.Clear();
			Process[] ps = FastCopy.ListupFastCopy();

			string[] sa = new string[ps.Length];

			for(int i=0; i < sa.Length; i++)
			{
				sa[i] = ps[i].ProcessName + ":" + ps[i].MainModule.FileName;
			}

			listBox1.Items.AddRange(sa);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			
		}

		private void button4_Click(object sender, EventArgs e)
		{
			FastCopyOptList fcl = new FastCopyOptList();

			FastCopyOpt fc = new FastCopyOpt();
			fc.src = tbSrc.Text;
			fc.dst = tbDst.Text;
			fc.args = tbOpt.Text;
			fc.caption = "001";
			fcl.Items.Add(fc);
			FastCopyOpt fc2 = new FastCopyOpt();
			fc2.src = tbSrc.Text;
			fc2.dst = tbDst.Text;
			fc2.args = tbOpt.Text;
			fc2.caption = "002";
			fcl.Items.Add(fc2);

			fcl.Save("Test.json");
			tbSrc.Text = "";
			tbDst.Text = "";
			tbOpt.Text = "";
			
		}

		private void button5_Click(object sender, EventArgs e)
		{
			FastCopyOptList fcl2 = new FastCopyOptList();
			fcl2.Load("Test.json");
			if (fcl2.Items.Count >= 2)
			{
				tbSrc.Text = fcl2.Items[1].src;
				tbDst.Text = fcl2.Items[1].dst;
				tbOpt.Text = fcl2.Items[1].args;
			}
		}

		private void button6_Click(object sender, EventArgs e)
		{
			FastCopyOpt fc = new FastCopyOpt();
			fc.src = tbSrc.Text;
			fc.dst = tbDst.Text;
			fc.args = tbOpt.Text;
			fc.caption = "001";

			tbArg.Text = fc.ToJsonD();
			tbArgD.Text = FastCopyOpt.DDecode(tbArg.Text);
		}
	}
}
