using Crawler.Utils.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Utils
{
    public class HtmlTag
    {
        /// <summary>
        /// 返回a标签
        /// </summary>
        /// <param name="aNode"></param>
        /// <returns></returns>
        public static Anchor GetAnchor(HtmlNode aNode)
        {
            if (aNode == null)
            {
                return null;
            }
            return new Anchor()
            {
                Text = TextUtils.ReplaceChar(aNode.InnerText),
                Href = aNode.GetAttributeValue("href", "")
            };
        }


        ///// <summary>
        ///// 返回a标签
        ///// </summary>
        ///// <param name="aNode"></param>
        ///// <returns></returns>
        //public static Anchor GetAnchor(HtmlNode aNode)
        //{
        //    if (aNode == null)
        //    {
        //        return null;
        //    }
        //    return new Anchor()
        //    {
        //        Text = TextUtils.ReplaceChar(aNode.InnerText),
        //        Href = aNode.GetAttributeValue("href", "")
        //    };
        //}




    }
}
