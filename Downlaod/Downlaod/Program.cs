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
        int i = 0;
        WebClient wci = new WebClient();
        static String cutOut(String n)
        {
            int first = n.IndexOf("http");
            int last = n.LastIndexOf("\"");
            string result = n.Substring(first, last - first);
            return result;
        }
        static String getSourceCode(String url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string srccode = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return srccode;
        }
         void downloads(String dl)
        {
            using (WebClient wci = new WebClient())
            {
                wci.Headers["Referer"] = "https://webtoon-phinf.pstatic.net";
                 wci.DownloadFile(dl, "img_" + i + ".jpg");
                Console.WriteLine("download " + i + "success");
                i++;
            }
        }
        static void Main(string[] args)
        {
            using(WebClient wc = new WebClient())
            {
                Program pg = new Program();
                wc.Headers["Referer"] = "https://webtoon-phinf.pstatic.net";
                String src = getSourceCode("https://www.webtoons.com/th/romance/gangnam-beauty/%E0%B8%95%E0%B8%AD%E0%B8%99%E0%B8%9E%E0%B9%80%E0%B8%A8%E0%B8%A9-ep02/viewer?title_no=792&episode_no=88");
                /*int last = _src.IndexOf("viewer_ad_area");
                int first = _src.IndexOf("_imageList");
                String src = _src.Substring(first,last-first);*/
                //string _pattern = @"/class=\"_images\" data-url=\"(.+)\"\\s(?=rel)/gim";
                string first = "class=\"";
                string image = "_images";
                string third = "\" data-url=\"(.+)\"\\s(?=rel)";
                string pattern = first+image+third;
                Regex rgx = new Regex(pattern);
                Match match = rgx.Match(src);
                if (match.Success)
                {
                    //Console.WriteLine(cutOut(match.Value));
                    pg.downloads(cutOut(match.Value));
                    foreach (Match m in rgx.Matches(src, match.Index + match.Length))
                        //Console.WriteLine(cutOut(m.Value));
                        pg.downloads(cutOut(m.Value));
                }
                
                
                Console.ReadKey();
                /*wc.Headers["Referer"] = "http://nekopost.net";
                    wc.DownloadFile("http://www.nekopost.net/file_server/collectManga/4823/46751/1.jpg", "test.jpg");
                    Console.WriteLine("Succes");
                    Console.ReadKey();*/
                //wc.Headers["Referer"] = "https://webtoon-phinf.pstatic.net";
                /*wc.Headers.Add("Referer", "https://webtoon-phinf.pstatic.net");
                    wc.DownloadFile("https://webtoon-phinf.pstatic.net/20180720_4/1532090383511O5k3e_JPEG/15320903834861356168.jpg?type=q90", "test.jpg");
                    Console.WriteLine("Succes");
                    Console.ReadKey();*/


            }
        }
    }
}
