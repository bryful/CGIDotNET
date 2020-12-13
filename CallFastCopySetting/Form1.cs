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

using System.Diagnostics;
using System.Runtime.InteropServices;//Marshalのために追加

using NFsCGI;

namespace CallFastCopySetting
{
	public partial class Form1 : Form
	{
		private FastCopyOptList m_FCOL = new FastCopyOptList();

		public Form1()
		{
			InitializeComponent();
		}

		private void tbExePath_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void tbExePath_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (files.Length <= 0) return;

			foreach(string s in files)
			{
				string n = Path.GetFileName(s).ToLower();
				if( n=="fastcopy.exe")
				{
					if(File.Exists(s))
					{
						tbExePath.Text = s;
						m_FCOL.ExePath = s;
						break;
					}
				}
			}


		}

		private void tbSrc_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (files.Length <= 0) return;


			TextBox tb = (TextBox)sender;
			foreach (string s in files)
			{
				if (Directory.Exists(s))
				{
					tb.Text = UniversalName.GetName(s);
					break;
				}
			}

		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			string c = tbCaption.Text.Trim();
			string s = tbSrc.Text.Trim();
			string d = tbDst.Text.Trim();
			string o = tbOpt.Text.Trim();
			if ((c == "") || (s == "") || (d == "")) return;

			FastCopyOpt fc = new FastCopyOpt();
			fc.caption = c;
			fc.src = s;
			fc.dst = d;
			fc.args = o;

			m_FCOL.Items.Add(fc);
			listBox1.Items.Add(c);
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox lb = (ListBox)sender;
			int si = lb.SelectedIndex;
			if ( si < 0) return;
			tbCaption.Text = m_FCOL.Items[si].caption;
			tbSrc.Text = m_FCOL.Items[si].src;
			tbDst.Text = m_FCOL.Items[si].dst;
			tbOpt.Text = m_FCOL.Items[si].args;

		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			m_FCOL.Save("fastcopy.json");
		}
		public bool Import(string p)
		{
			bool ret = false;

			listBox1.Items.Clear();
			m_FCOL.Items.Clear();

			ret = m_FCOL.Load(p);
			if(ret)
			{
				if(m_FCOL.Items.Count>0)
				{
					foreach(FastCopyOpt a in m_FCOL.Items)
					{
						listBox1.Items.Add(a.caption);
					}
					ret = true;
				}
				tbExePath.Text = m_FCOL.ExePath;
			}

			return ret;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Import("fastcopy.json");
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			ListBox lb = (ListBox)sender;
			int si = lb.SelectedIndex;
			if (si < 0) return;
			listBox1.Items.RemoveAt(si);
			m_FCOL.Items.RemoveAt(si);


		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			int si = listBox1.SelectedIndex;
			if (si < 0) return;
			string c = tbCaption.Text.Trim();
			string s = tbSrc.Text.Trim();
			string d = tbDst.Text.Trim();
			string o = tbOpt.Text.Trim();
			if ((c == "") || (s == "") || (d == "")) return;

			FastCopyOpt fc = new FastCopyOpt();
			fc.caption = c;
			fc.src = s;
			fc.dst = d;
			fc.args = o;

			m_FCOL.Items[si] = fc;
			listBox1.Items[si] = fc.caption;
		}
		// ********************************************************************

	}
}
