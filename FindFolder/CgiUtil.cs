using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Threading;

namespace FindFolder
{
    public enum CGI_MODE
    {
        None = 0,
        GET,
        POST,
        ARGS,

    }
    public class CgiData
    {
        public string Tag;
        public string Value;
        public CgiData()
        {
            Init();
        }
        public CgiData(string s)
        {
            Analysis(s);
        }
        public void Analysis(string s)
        {
            Init();
            if (s == "") return;
            string[] sa = s.Split('=');
            if(sa.Length>=2)
            {
                Tag = sa[0];
                Value = sa[1];
            }
        }
        public void Init()
        {
            Tag = "";
            Value = "";
        }
    }
    public class CgiDataList
    {
        public List<CgiData> Data = new List<CgiData>();
        public void SetData( string s)
        {
            
            Data.Clear();
            if (s == "") return;
            string[] sa = new string[0];

            try
            {
                if (s.IndexOf("&") >= 0)
                {
                    sa = s.Split('&');
                    if (sa.Length <= 0) return;
                    foreach (string p in sa)
                    {
                        CgiData cd = new CgiData(p);
                        if (cd.Tag != "")
                        {
                            Data.Add(cd);
                        }
                    }

                }
                else
                {
                    CgiData cd2 = new CgiData(s);
                    if (cd2.Tag != "")
                    {
                        Data.Add(cd2);
                    }
                }
            }
            catch
            {

            }
        }
        public CgiData this[int idx]
        {
            get 
            {
                return Data[idx];
            }
            set
            {
                Data[idx] = value;
            }
        }
        public bool FindValueFromTag(string tag,out string v)
        {
            bool ret = false;
            v = "";
            if(Data.Count>0)
            {
                foreach(CgiData cd in Data)
                {
                    if (cd.Tag == tag)
                    {
                        v = cd.Value;
                        ret = true;
                    }
                }
            }
            return ret;
        }
    }
    public class CgiUtil
    {
        // **************************************************************
        private CGI_MODE m_Mode = CGI_MODE.None;
        public CGI_MODE Mode{get { return m_Mode; }}
        // **************************************************************
        private string m_PATH_INFO = "";
        public string PATH_INFO { get { return m_PATH_INFO; } }
        private string m_QUERY_STRING = "";
        public string QUERY_STRING { get { return m_QUERY_STRING; } }
        private string [] m_ARGS = new string[0];
        public string [] ARGS { get { return m_ARGS; } }
        // **************************************************************
        private string m_lockFile = "";
        private FileStream m_lockStream = null;
        // **************************************************************
        private static string HtmlHeader = "Content-type: text/html\n\n";
        // **************************************************************
        public CgiDataList Data = new CgiDataList();
        // **************************************************************
        public CgiUtil(string[] args)
        {
            m_PATH_INFO = HttpUtility.UrlDecode(System.Environment.GetEnvironmentVariable("PATH_INFO"));
            m_QUERY_STRING = HttpUtility.UrlDecode(Environment.GetEnvironmentVariable("QUERY_STRING"));
            if(args.Length>0)
            {
                m_ARGS = args;
            }
            string REQUEST_METHOD = System.Environment.GetEnvironmentVariable("REQUEST_METHOD");
            bool IsGET = (REQUEST_METHOD == "GET");
            bool IsPOST = (REQUEST_METHOD == "POST");
            bool IsARGS = (args.Length > 0);

            Data.Data.Clear();


            if (IsGET)
            {
                m_Mode = CGI_MODE.GET;
                Data.SetData(m_QUERY_STRING);
            }
            else if (IsPOST)
            {
                m_Mode = CGI_MODE.POST;

                int CL = int.Parse(System.Environment.GetEnvironmentVariable("CONTENT_LENGTH"));
                if (CL > 0)
                {
                    Stream inputStream = Console.OpenStandardInput();
                    byte[] bytes = new byte[CL];
                    int outputLength = inputStream.Read(bytes, 0, CL);
                    char[] chars = Encoding.UTF7.GetChars(bytes, 0, outputLength);
                    m_QUERY_STRING = HttpUtility.UrlDecode(new string(chars));
                    Data.SetData(m_QUERY_STRING);
                }
            }
            else if (IsARGS)
            {
                m_Mode = CGI_MODE.ARGS;
            }

        }
        // **************************************************************
        public void WriteHtml(string s)
        {
            Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine(HtmlHeader);
            Console.WriteLine(s);

        }
        // **************************************************************
        public void WriteErr()
        {
            Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine(HtmlHeader);
            Console.WriteLine("CGI Error!");
        }
        // **************************************************************
        protected virtual bool IsFileLocked(FileInfo file)
        {
            if (file.Exists == false) return false;

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
        // **************************************************************
        public bool CreateLockFile(string nm)
        {

            bool ret = false;
            m_lockFile = Path.Combine(Path.GetTempPath(), nm);
            if (File.Exists(m_lockFile) == true) return ret;

            m_lockStream = null;
            //0byteのファイルを作る
            File.Create(m_lockFile).Close();

            if (File.Exists(m_lockFile) == true)
            {
                //ロックする
                m_lockStream = new FileStream(
                    m_lockFile,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read,
                    System.IO.FileShare.None);
                ret = true;
            }

            return ret;
        }
        // **************************************************************
        public bool CloseLockFile()
        {
            bool ret = false;
            if (File.Exists(m_lockFile) == false) return ret;

            if(m_lockStream!=null )
            {
                m_lockStream.Close();
                File.Delete(m_lockFile);
                m_lockStream = null;
                ret = true;
            }
            return ret;
        }
        // **************************************************************
        public bool CheckLockFile(string nm)
        {
            bool ret = false;
            m_lockFile = Path.Combine(Path.GetTempPath(), nm);
            FileInfo fi = new FileInfo(m_lockFile);

            int cnt = 0;
            while(IsFileLocked(fi))
            {
                cnt++;
                Thread.Sleep(100);
                if (cnt > 20) return ret;
            }

            ret = CreateLockFile(nm);

            return ret;
        }

    }
}
