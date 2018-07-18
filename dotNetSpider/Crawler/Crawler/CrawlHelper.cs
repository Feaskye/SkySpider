using Crawler.Models;
using Crawler.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            var categories = CrawCategories("list.html");
            foreach (var cate in categories)
            {
                var articles = CrawlArticles(cate.Url);
            }
        }





        public static List<Category> CrawCategories(string cateUrl)
        {
            List<Category> categories = new List<Category>();
            var rootnode = HtmlNoder.GetHtmlRoot(WebDomain + cateUrl);
            string cateHtmlPath = "//table[@class='catetable']/tr";//"//span[@class='num']/font[last()]";
            HtmlNodeCollection cateHtml = rootnode.SelectNodes(cateHtmlPath);    //所有找到的节点都是一个集合
            if (cateHtml != null)
            {
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
                    }
                    categories.Add(category);
                    Console.WriteLine(category.Name + "：" + category.Url);
                    Console.WriteLine(string.Join(" ", category.Childs.Select(u => u.Name)));
                    Console.WriteLine("--------------------------------------------");
                }
            }
            return categories;
        }




        public static List<Article> CrawlArticles(string listUrl)
        {
            var articleQueues = new ConcurrentQueue<Article>();
            var rootNode = HtmlNoder.GetHtmlRoot(listUrl);//考虑分页
            //取出总页数
            var spanPager = rootNode.SelectSingleNode("//td[@class='pageTotal']/span[2]").InnerText();
            var totalPage = spanPager.ToInt();

            Parallel.For(1, totalPage, (curr) => {
                //"c-0001500001-1-5404-0-0-0-0-9-0-0.html";
                var lurl = listUrl.Replace(".html", $"-{curr}-5404-0-0-0-0-9-0-0.html");
                rootNode = HtmlNoder.GetHtmlRoot(lurl);//考虑分页
                //列表页数据
                var articleNodes = rootNode.SelectNodes("//div[@class='doc-list']/ul/li");
                foreach (var articleNode in articleNodes)
                {
                    var aNode = articleNode.SelectSingleNode("div[@class='doc-list-title']/h3/a");
                    var art = HtmlTag.GetAnchor(aNode);

                    var article = new Article() { Name = art.Text,Url = art.Href};
                    Console.WriteLine($" >>>{art.Text}  ：{art.Href}");

                    var articleRootNode = HtmlNoder.GetHtmlRoot(WebDomain + art.Href);
                    article.Content = articleRootNode.SelectSingleNode("//dl/dd[@class='fLeft wordwrap']").InnerText();
                    article.FileType =  FileType.doc;
                    article.AttachFiles =new string[] { "www.baidu.com"} ;

                    articleQueues.Enqueue(article);
                }
            });

            return articleQueues.ToList();

        }



        private static Category GetCategory(HtmlNode htmlNode)
        {
            var tag = HtmlTag.GetAnchor(htmlNode);
            return new Category { Name = tag.Text,Url =WebDomain+ tag.Href};
        }
        
        
    }
}
