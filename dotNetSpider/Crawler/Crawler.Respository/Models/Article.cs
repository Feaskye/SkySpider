using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Crawler.Respository
{
    [Table("Article")]
    public class Article : ModelEntity
    {
        public string Title { get; set; }

        public string Keyword { get; set; }
        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadCount { get; set; }

        public int PageCount { get; set; }

        public int RequireAmount { get; set; }

        public string ResourceType { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// 原文链接
        /// </summary>
        public string ResourceUrl { get; set; }
        /// <summary>
        /// 分类编号
        /// </summary>
        public string CategoryId { get; set; }


        public int BookStatus { get; set; }
        public string CoverUrl { get; set; }
    }
}
