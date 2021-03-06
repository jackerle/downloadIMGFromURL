﻿using System;
using System.Web;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;



namespace Downlaod
{
    class Program
    {
        int i = 0;                  //page count
        WebClient wci = new WebClient();
        static string path;             //directory for img
        static String cutOut(String n)
        {
            int first = n.IndexOf("http");
            int last = n.LastIndexOf("\"");                 //this patten for webtoon
            string result = n.Substring(first, last - first);
            return result;
        }
        static String getSourceCode(String url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());            // for req and res for get srccode from url
            string srccode = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return srccode;
        }
         void downloads(String dl)
        {
            using (WebClient wci = new WebClient())
            {
                wci.Headers["Referer"] = "https://webtoon-phinf.pstatic.net";           //setHeader
                 wci.DownloadFile(dl, path+"\\img_" + i + ".jpg");                      //download file (url,path)
                Console.WriteLine("download " + i + "success");
                i++;
            }
        }
        static void mkeDirectory(string name)
        {
            path = "E:\\IMGfromURL\\" + name;
            DirectoryInfo di = Directory.CreateDirectory(path);
        }
        static void Main(string[] args)
        {
            using(WebClient wc = new WebClient())
            {
                Console.Write("input URL :");
                string _url = Console.ReadLine();               //read url
                Console.Write("input folderName :");
                string _name = Console.ReadLine();              //read name for folder
                mkeDirectory(_name);                            //make dir
                Program pg = new Program();
                wc.Headers["Referer"] = "https://webtoon-phinf.pstatic.net";
                String src = getSourceCode(_url);               //getSrccode
                string first = "class=\"";  string image = "_images"; string third = "\" data-url=\"(.+)\"\\s(?=rel)"; // this for webtoon
                string pattern = first+image+third;
                Regex rgx = new Regex(pattern);                 //use regex for cut string
                Match match = rgx.Match(src);
                if (match.Success)
                {
                    pg.downloads(cutOut(match.Value));
                    foreach (Match m in rgx.Matches(src, match.Index + match.Length))
                        pg.downloads(cutOut(m.Value));
                }
                Console.WriteLine("All download has success!");
                Console.WriteLine("Press any key to continue");
                
                Console.ReadKey();
            }
        }
    }
}
