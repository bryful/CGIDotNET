﻿
namespace ExeTest
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbArgD = new System.Windows.Forms.TextBox();
			this.tbArg = new System.Windows.Forms.TextBox();
			this.button6 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.tbOpt = new System.Windows.Forms.TextBox();
			this.tbDst = new System.Windows.Forms.TextBox();
			this.tbSrc = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 32;
			this.listBox1.Location = new System.Drawing.Point(12, 12);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(922, 196);
			this.listBox1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(942, 14);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 58);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.textBox1.Location = new System.Drawing.Point(14, 214);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(741, 39);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "C:\\Bin\\FastCopy\\FastCopy.exe";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(767, 214);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(167, 39);
			this.button2.TabIndex = 3;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// textBox2
			// 
			this.textBox2.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.textBox2.Location = new System.Drawing.Point(12, 259);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(882, 39);
			this.textBox2.TabIndex = 4;
			this.textBox2.Text = "C:\\Bin\\FastCopy\\FastCopy.exe";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(916, 264);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(167, 39);
			this.button3.TabIndex = 5;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tbArgD);
			this.groupBox1.Controls.Add(this.tbArg);
			this.groupBox1.Controls.Add(this.button6);
			this.groupBox1.Controls.Add(this.button5);
			this.groupBox1.Controls.Add(this.button4);
			this.groupBox1.Controls.Add(this.tbOpt);
			this.groupBox1.Controls.Add(this.tbDst);
			this.groupBox1.Controls.Add(this.tbSrc);
			this.groupBox1.Location = new System.Drawing.Point(14, 304);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(614, 229);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			// 
			// tbArgD
			// 
			this.tbArgD.Location = new System.Drawing.Point(19, 185);
			this.tbArgD.Name = "tbArgD";
			this.tbArgD.Size = new System.Drawing.Size(450, 23);
			this.tbArgD.TabIndex = 7;
			this.tbArgD.Text = "/aaa /bbb";
			// 
			// tbArg
			// 
			this.tbArg.Location = new System.Drawing.Point(19, 156);
			this.tbArg.Name = "tbArg";
			this.tbArg.Size = new System.Drawing.Size(450, 23);
			this.tbArg.TabIndex = 6;
			this.tbArg.Text = "/aaa /bbb";
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(475, 156);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 23);
			this.button6.TabIndex = 5;
			this.button6.Text = "button6";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(475, 92);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 4;
			this.button5.Text = "button5";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(475, 63);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 3;
			this.button4.Text = "button4";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// tbOpt
			// 
			this.tbOpt.Location = new System.Drawing.Point(24, 63);
			this.tbOpt.Name = "tbOpt";
			this.tbOpt.Size = new System.Drawing.Size(420, 23);
			this.tbOpt.TabIndex = 2;
			this.tbOpt.Text = " /cmd=sync /force_close /no_confirm_del /open_window";
			// 
			// tbDst
			// 
			this.tbDst.Location = new System.Drawing.Point(310, 22);
			this.tbDst.Name = "tbDst";
			this.tbDst.Size = new System.Drawing.Size(282, 23);
			this.tbDst.TabIndex = 1;
			this.tbDst.Text = "\\\\192.168.10.88\\sv04\\";
			// 
			// tbSrc
			// 
			this.tbSrc.Location = new System.Drawing.Point(24, 22);
			this.tbSrc.Name = "tbSrc";
			this.tbSrc.Size = new System.Drawing.Size(280, 23);
			this.tbSrc.TabIndex = 0;
			this.tbSrc.Text = "E:\\pool2\\";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1120, 665);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.listBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.TextBox tbOpt;
		private System.Windows.Forms.TextBox tbDst;
		private System.Windows.Forms.TextBox tbSrc;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.TextBox tbArg;
		private System.Windows.Forms.TextBox tbArgD;
	}
}

