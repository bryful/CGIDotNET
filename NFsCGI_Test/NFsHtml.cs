using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Threading;

namespace NFsCGI
{
	public class NFsHtml
	{
		public readonly string[] TagDef = new string[]
		{
			"SERVER_SOFTWARE",
			"SERVER_NAME",
			"GATEWAY_INTERFACE",
			"SERVER_PROTOCOL",
			"SERVER_PORT",
			"REQUEST_METHOD",
			"PATH_INFO",
			"PATH_TRANSLATED",
			"SCRIPT_NAME",
			"QUERY_STRING",
			"REMOTE_HOST",
			"REMOTE_ADDR",
			"AUTH_TYPE",
			"REMOTE_USER",
			"REMOTE_IDENT",
			"CONTENT_TYPE",
			"CONTENT_LENGTH",
			"HTTP_ACCEPT",
			"HTTP_USER_AGENT"
		};
		public readonly string HtmlHeader = "Content-type: text/html\n\n";

		private string m_Html = "";
		public string Html
		{
			get { return m_Html; }
			set { m_Html = value; }
		}
		private List<List<string>> m_TagTable = new List<List<string>>();
		public NFsHtml()
		{
			Init();
		}
		public NFsHtml(string p)
		{
			Load(p);
		}
		// **************************************************************
		public void Init()
		{
			m_Html = "";
			m_TagTable.Clear();
		}
		// **************************************************************
		public void ReplaceTag(string tag , string v)
		{
			m_Html = m_Html.Replace(tag,v);
		}
		// **************************************************************
		public bool Load(string p)
		{
			bool ret = false;

			if (File.Exists(p) == true)
			{
				Init();
				m_Html = File.ReadAllText(p, Encoding.GetEncoding("utf-8"));
			}
			return ret;
		}
		// **************************************************************
		public bool Save(string p)
		{
			bool ret = false;

			try
			{
				File.WriteAllText(p, m_Html, Encoding.GetEncoding("utf-8"));
				ret = File.Exists(p);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		// **************************************************************
		public void output()
		{
			Console.OutputEncoding = new UTF8Encoding();
			Console.WriteLine(HtmlHeader);
			Console.WriteLine(m_Html);
		}
		// **************************************************************
	}
}
