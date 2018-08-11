using Crawler.Models;
using Crawler.Respository;
using Crawler.Utils;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
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
        public static ICrawlDbResposity<ArticleCategory> cateResposity;
        private static ServiceProvider m_ServiceProvide;

        /// <summary>
        /// 思路：获取分类、循环、数据列表并翻页（逐个进入详细页）
        /// </summary>
        public static void Run(ServiceProvider serviceProvider)
        {
            m_ServiceProvide = serviceProvider;
            cateResposity = serviceProvider.GetService<ICrawlDbResposity<ArticleCategory>>();

            var categories = CrawCategories("list.html");
            foreach (var cate in categories)
            {
                try
                {
                    var articles = CrawlArticles(cate.Id, cate.ShortTitle);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }





        public static List<ArticleCategory> CrawCategories(string cateUrl)
        {
            List<ArticleCategory> categories = new List<ArticleCategory>();
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
                    //category.Childs = new List<ArticleCategory>();

                    cateResposity.AddCategory(category);
                    Console.WriteLine(category.Title + "：" + category.ShortTitle);

                    categories.Add(category);
                    foreach (var p2Node in cate2Nodes)
                    {
                        var child = GetCategory(p2Node);
                        child.ParentId = category.Id;
                        cateResposity.AddCategory(child);
                        categories.Add(child);
                        Console.WriteLine(">>>>>>" + child.Title + "：" + child.ShortTitle);
                    }
                    
                    Console.WriteLine("--------------------------------------------");
                }
            }
            return categories;
        }




        public static List<Article> CrawlArticles(string cateId,string listUrl)
        {
           
            var articleQueues = new ConcurrentQueue<Article>();
            var rootNode = HtmlNoder.GetHtmlRoot(listUrl);//考虑分页
            //取出总页数
            var spanPager = rootNode.SelectSingleNode("//td[@class='pageTotal']/span[2]").InnerText();
            var totalPage = spanPager.ToInt();

            var pageNode = rootNode.SelectSingleNode("//div[@class='paginator']/span[@class='cpb']");
            var pageUrl = pageNode == null ? null : HtmlTag.GetAnchor(pageNode.NextSibling).Href;
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                return null;
            }
            var lurl = listUrl.Replace(WebDomain,"/").Replace(".html", "");

            Parallel.For(1, totalPage, curr => {

                try
                {
                    //"c-0001500001-1-5404-0-0-0-0-9-0-0.html";
                    var currUrl = lurl + "-" + curr + pageUrl.Substring(lurl.Length + 2);
                    CrawlArticleList(articleQueues, currUrl, cateId);

                    //var nextPageNode = rootNode.SelectSingleNode("//div[@class='paginator']/span[@class='cpb']").NextSibling;
                    //if (nextPageNode != null)
                    //{
                    //    nextUrl = HtmlTag.GetAnchor(nextPageNode).Href;
                    //}

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            
            var articleResposity = m_ServiceProvide.GetService<ICrawlDbResposity<Article>>();
            var titles = articleQueues.Select(u => u.Title).ToArray();
            var allTitles = articleResposity.Get(u => u.Title, u => true).ToList();

            var leaveTitltes = titles.Where(w => !allTitles.Contains(w));
            var newArticleList = articleQueues.Where(w => leaveTitltes.Contains(w.Title)).ToList();
            if (newArticleList.Any())
            {
                articleResposity.AddBetch(newArticleList, 100);
            }

            return articleQueues.ToList();

        }



        public static void CrawlArticleList(ConcurrentQueue<Article> articleQueues,string currUrl,string cateId)
        {
            var rootNode = HtmlNoder.GetHtmlRoot(WebDomain + currUrl);
            if (rootNode == null)
                return;
            //列表页数据
            var articleNodes = rootNode.SelectNodes("//div[@class='doc-list']/ul/li");
            if (articleNodes == null || articleNodes.Count == 0)
            {
                Console.WriteLine($" >>>{currUrl}  ：{cateId}");
            }
            Parallel.ForEach(articleNodes, articleNode =>
            {
                //foreach (var articleNode in articleNodes)
                //{
                try
                {
                    var aNode = articleNode.SelectSingleNode("div[@class='doc-list-title']/h3/a");
                    var art = HtmlTag.GetAnchor(aNode);
                    var page = articleNode.SelectSingleNode("div[@class='doc-list-info']/div[@class='page']/strong").InnerText();
                    var readCount = articleNode.SelectSingleNode("div[@class='doc-list-comment']/div[@class='read']").InnerText().Replace("人已阅读", "");
                    var amount = articleNode.SelectSingleNode("div[@class='doc-list-info']/div[@class='price ticket']/span").InnerText();
                    var type = articleNode.SelectSingleNode("div[@class='doc-list-title']/h3/img").GetAttributeValue("class");
                    var coverUrl = articleNode.SelectSingleNode("div[@class='doc-list-img']/a/img").GetAttributeValue("src");

                    var article = new Article()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = art.Text,
                        ResourceUrl = WebDomain + art.Href,
                        CategoryId = cateId,
                        CreatorTime = DateTime.Now,
                        BookStatus = 1,
                        SortCode = 1,
                        Keyword = art.Text,
                        PageCount = Convert.ToInt32(page),
                        ReadCount = Convert.ToInt32(readCount),
                        RequireAmount = Convert.ToInt32(amount),
                        ResourceType = type,
                        CoverUrl = coverUrl
                    };

                    var articleRootNode = HtmlNoder.GetHtmlRoot(WebDomain + art.Href);
                    if (articleRootNode == null)
                        return;
                    article.Description = articleRootNode.SelectSingleNode("//dl/dd[@class='fLeft wordwrap']").InnerText();
                    //var filesNodes = articleRootNode.SelectNodes("//div[class='outer_page']/div[class='inner_page']/div/img");
                    //var files = new List<string>();
                    //foreach (var imgNode in filesNodes)
                    //{
                    //    files.Add(imgNode.GetAttributeValue("src", ""));
                    //}
                    ////article.FileType =  FileType.doc;
                    //article.Attachment = string.Join(";", files);

                    articleQueues.Enqueue(article);

                    Console.WriteLine($" >>>{art.Text}  ：{WebDomain + art.Href}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //}
            });
        }





        private static ArticleCategory GetCategory(HtmlNode htmlNode)
        {
            var tag = HtmlTag.GetAnchor(htmlNode);
            return new ArticleCategory { Id = Guid.NewGuid().ToString(), Title = tag.Text, ShortTitle = WebDomain + tag.Href, Keyword = tag.Text, CreatorTime=DateTime.Now };
        }
        
        
    }
}
