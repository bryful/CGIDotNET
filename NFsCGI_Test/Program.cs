using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFsCGI;

namespace NFsCGI_Test
{
	class Program
	{
		static void Main(string[] args)
		{
			NFsCgi Cgi = new NFsCgi(args);

			NFsHtml html = new NFsHtml("./NFsCGI_Test.html");

			html.ReplaceTag("$PATH_INFO", Cgi.PATH_INFO);
			html.ReplaceTag("$QUERY_STRING", Cgi.QUERY_STRING);
			html.ReplaceTag("$Data", Cgi.Data.ToJson());


			html.output();

		}
	}
}
