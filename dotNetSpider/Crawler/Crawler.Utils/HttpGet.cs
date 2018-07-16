using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Crawler.Utils
{
    public class HttpGet
    {
        public static string GetHtml(string url)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                WebRequest rGet = WebRequest.Create(url);
                WebResponse rSet = rGet.GetResponse();
                Stream s = rSet.GetResponseStream();
                using (StreamReader reader = new StreamReader(s, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException)
            {
                //连接失败
                return null;
            }
        }
    }
}
