using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Crawler
{
    /// <summary>
    /// http://www.cnblogs.com/bomo/archive/2013/01/28/2879361.html
    /// </summary>
    class CrawlHelper
    {

        public static void Run(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            HtmlNode rootnode = doc.DocumentNode;
        }

        public static void Run()
        {
            string htmlstr = GetHtmlStr("http://www.hao123.com");
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlstr);
            HtmlNode rootnode = doc.DocumentNode;    //XPath路径表达式，这里表示选取所有span节点中的font最后一个子节点，其中span节点的class属性值为num
                                                     //根据网页的内容设置XPath路径表达式
            string xpathstring = "//span[@class='num']/font[last()]";
            HtmlNodeCollection aa = rootnode.SelectNodes(xpathstring);    //所有找到的节点都是一个集合

            if (aa != null)
            {
                string innertext = aa[0].InnerText;
                string color = aa[0].GetAttributeValue("color", "");    //获取color属性，第二个参数为默认值
                                                                        //其他属性大家自己尝试
            }
        }

        public static string GetHtmlStr(string url)
        {
            try
            {
                WebRequest rGet = WebRequest.Create(url);
                WebResponse rSet = rGet.GetResponse();
                Stream s = rSet.GetResponseStream();
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (WebException)
            {
                //连接失败
                return null;
            }
        }
    }
}
