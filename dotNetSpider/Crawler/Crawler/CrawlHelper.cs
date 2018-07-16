using Crawler.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Crawler
{
    /// <summary>
    /// http://www.cnblogs.com/bomo/archive/2013/01/28/2879361.html
    /// </summary>
    class CrawlHelper
    {
        public static string WebDomain = "http://www.oilwenku.com/";

        /// <summary>
        /// 思路：获取分类、循环、数据列表并翻页（逐个进入详细页）
        /// </summary>
        public static void Run()
        {
            HtmlNode rootnode = GetHtmlRoot(WebDomain + "list.html");
            string cateHtmlPath = "//table[@class='catetable']/tr";//"//span[@class='num']/font[last()]";
            HtmlNodeCollection cateHtml = rootnode.SelectNodes(cateHtmlPath);    //所有找到的节点都是一个集合
            if (cateHtml != null)
            {
                List<Category> categories = new List<Category>();
                foreach (var p1 in cateHtml)
                {
                    var catePNode = p1.SelectSingleNode("td/div/a[@class='f1']");
                    var category = GetCategory(catePNode);
                    
                    var cate2Nodes = p1.SelectNodes("td/div/a[@class='f2']");
                    category.Childs = new List<Category>();
                    foreach (var p2Node in cate2Nodes)
                    {
                        var child = GetCategory(p2Node);
                        category.Childs.Add(child);
                        //取列表页
                        System.Threading.Thread.Sleep(2000);
                        rootnode = GetHtmlRoot(child.Url);//考虑分页
                        var articleNodes = rootnode.SelectNodes("//div[@class='doc-list']/ul/li");
                        foreach (var articleNode in articleNodes)
                        {
                            var aNode =articleNode.SelectSingleNode("div[@class='doc-list-title']/h3/a");
                            Console.WriteLine($" >{aNode.InnerText.Replace(" ","").Replace("\r","")}");
                        }
                    }
                    categories.Add(category);
                    Console.WriteLine(category.Name + "："+category.Url);
                    Console.WriteLine(string.Join(" ", category.Childs.Select(u => u.Name)));
                    Console.WriteLine("--------------------------------------------");
                }

           
            }
        }


        public static Category GetCategory(HtmlNode aNode)
        {
            return new Category()
            {
                Name = aNode.InnerText.Replace("\r","").Replace("\n", "").Replace(" ",""),
                Url = WebDomain + aNode.GetAttributeValue("href", "")
            };
        }


        public static HtmlNode GetHtmlRoot(string url)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                if (doc.DocumentNode != null)
                {
                    return doc.DocumentNode;
                }
                System.Threading.Thread.Sleep(1000);
                WebRequest rGet = WebRequest.Create(url);
                WebResponse rSet = rGet.GetResponse();
                Stream s = rSet.GetResponseStream();
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                doc = new HtmlDocument();
                doc.LoadHtml(reader.ReadToEnd());
                return doc.DocumentNode;
            }
            catch (WebException)
            {
                //连接失败
                return null;
            }
        }
    }
}
