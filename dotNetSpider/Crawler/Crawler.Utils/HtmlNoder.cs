using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Utils
{
    public static class HtmlNoder
    {
        public static HtmlNode GetHtmlRoot(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            if (doc.DocumentNode != null)
            {
                return doc.DocumentNode;
            }

            doc = new HtmlDocument();
            doc.LoadHtml(HttpGet.GetHtml(url));
            return doc.DocumentNode;
        }


        public static string InnerText(this HtmlNode htmlNode)
        {
            return htmlNode == null ? "" : htmlNode.InnerText;
        }

        public static string InnerHtml(this HtmlNode htmlNode)
        {
            return htmlNode == null ? "" : htmlNode.InnerHtml;
        }















    }













}
