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

namespace NFsCGI
{
    public enum CMD
    {
        none = 0,
        diff,
        update,
        force_copy,
        sync,
        move,
        delete
	}

    public class FastCopyOpt
    {
        public string src = "";
        public string dst = "";
        public string args = "";
        public string caption = "";
        public FastCopyOpt()
        {
            Init();
        }
        public void Init()
        {
            src = dst = args = "";
            caption = "";
        }
        public string Options()
		{
            return args + " " + src + " /to=\"" + dst + "\"";
		}
        public string ToJson()
		{
            return ((DynamicJson)ToObj()).ToString();
        }
        public string ToJsonD()
        {
            //{"src":"C:\\AAA\\","dst":"D:\\BBB\\","args":"\/aaa \/bbb","caption":"001"}

            return DEncode(ToJson());
        }
        public static string DEncode(string s)
        {
            s = s.Replace("\"", "%Q");
            s = s.Replace(" ", "%S");
            s = s.Replace("\\", "%Y");
            s = s.Replace(",", "%K");
            s = s.Replace(":", "%C");
            s = s.Replace("/", "%L");
            s = s.Replace("{", "%1");
            s = s.Replace("}", "%2");
            s = s.Replace("[", "%3");
            s = s.Replace("]", "%4");
            return s;

        }
        public static string DDecode(string s)
		{
            s = s.Replace("%Q", "\"");
            s = s.Replace("%S", " ");
            s = s.Replace("%Y", "\\");
            s = s.Replace("%K", ",");
            s = s.Replace("%C", ":");
            s = s.Replace("%L", "/");
            s = s.Replace("%1", "{");
            s = s.Replace("%2", "}");
            s = s.Replace("%3", "[");
            s = s.Replace("%4","]");
            return s;

        }

        public string [] ToArray()
        {
            string[] ret = new string[4];
            ret[0] = caption;
            ret[1] = src;
            ret[2] = dst;
            ret[3] = args;
            return ret;

        }
        public void FromArray(string [] sa)
		{
            if (sa.Length >= 4)
            {
                caption = sa[0];
                src = sa[1];
                dst = sa[2];
                args = sa[3];
            }
        }
        public void FromJson(string s)
		{
            dynamic obj = DynamicJson.Parse(s);
            DynamicJson dobj = (DynamicJson)obj;
            string key = "src";
            if( dobj.IsDefined(key)) src = obj[key];
            key = "dst";
            if (dobj.IsDefined(key)) dst = obj[key];
            key = "args";
            if (dobj.IsDefined(key)) args = obj[key];
            key = "caption";
            if (dobj.IsDefined(key)) caption = obj[key];
        }
        public void FromJsonD(string s)
		{
            FromJson(DDecode(s));

        }

        public dynamic ToObj()
		{
            dynamic obj = new DynamicJson();
            obj["src"] = src;
            obj["dst"] = dst;
            obj["args"] = args;
            obj["caption"] = caption;
            return obj;
        }

    }
    public class FastCopyOptList
	{
        public List<FastCopyOpt> Items = new List<FastCopyOpt>();
        public string ExePath = "";
        public FastCopyOptList()
		{

		}
        public string ToJson2()
		{
            string ret = "";
            if(Items.Count>0)
			{
                for (int i=0; i< Items.Count; i++)
				{
                    if (ret != "") ret += ",\r\n";
                    ret += Items[i].ToJson();
                }
            }


            ret = "{\"FastCopy\":[\r\n" + ret + "\r\n]}\r\n";
            return ret;

		}
        public string ToJson()
        {
            string ret = "";

            if (Items.Count > 0)
            {
                string [][] objs = new string[Items.Count][];
                for (int i = 0; i < Items.Count; i++)
                {
                    objs[i] = Items[i].ToArray();
                }

                dynamic b = new DynamicJson();
                b["FastCopy"] = objs;
                b["ExePath"] = ExePath;
                ret = ((DynamicJson)b).ToString();
            }


            return ret;

        }
        public bool FromJson(string js)
		{
            bool ret = false;
            Items.Clear();
            if (js == "") return ret;
            dynamic obj = DynamicJson.Parse(js);
            DynamicJson dobj = (DynamicJson)obj;
            string key = "FastCopy";
            if (dobj.IsDefined(key))
			{
                if( ((DynamicJson)obj[key]).IsArray )
				{
                    dynamic[] ary = obj[key];
                    if(ary.Length>0)
					{
                        foreach(dynamic d in ary)
						{
                            if (((DynamicJson)d).IsArray)
                            {
                                string[] sa = (string[])d;
                                FastCopyOpt fc = new FastCopyOpt();
                                fc.FromArray(sa);
                                if ((fc.src != "") && (fc.dst != ""))
                                {
                                    Items.Add(fc);
                                }
                            }
						}
					}

                }
			}
            key = "ExePath";
            if (dobj.IsDefined(key))
            {
                ExePath = obj[key];
            }
            ret = (Items.Count > 0);
            return ret;
		}

        public bool Save(string p)
		{
            bool ret = false;

            string js = ToJson();
            if (js == "") return ret;
            try
            {
                File.WriteAllText(p, js, Encoding.GetEncoding("utf-8"));
                ret = File.Exists(p);
            }
            catch
            {
                ret = false;
            }

            return ret;
		}
        // **************************************************************
        public bool Load(string p)
        {
            bool ret = false;

            if (File.Exists(p) == true)
            {
                Items.Clear();
                string js = File.ReadAllText(p, Encoding.GetEncoding("utf-8"));
                if(js !="")
				{
                    try
                    {
                        ret = FromJson(js);
                    }
                    catch
                    {
                        ret = false;
                    }
				}
            }
            return ret;
        }

    }
    public class FastCopy
	{
        static public Process [] ListupFastCopy()
        {
            return Process.GetProcessesByName("FastCopyCall");
        }
        static public string [] ListUpProcess()
		{
            string[] ret = new string[0];

            Process[] ps = Process.GetProcesses();
            if (ps.Length <= 0) return ret;

            ret = new string[ps.Length];
            for (int i=0; i<ps.Length;i++)
            {
                try
                {
                    Process p = ps[i];

                    string s = p.ProcessName; //+ ":" + p.MainModule.FileName + String.Format("{0}", p.TotalProcessorTime);
                    ret[i] = s;

                }
                catch 
                {
                }
            }
            return ret;
        }
        public static bool  IsFastCopy()
        {
            bool ret = false;

            Process[] ps = Process.GetProcesses();
            if (ps.Length <= 0) return ret;

            for (int i = 0; i < ps.Length; i++)
            {
                try
                {
                    Process p = ps[i];
                    if(p.ProcessName == "FastCopy")
					{
                        ret = true;
                        break;
                    }

                }
                catch
                {
                }
            }
            return ret;
        }
    

    }
}
