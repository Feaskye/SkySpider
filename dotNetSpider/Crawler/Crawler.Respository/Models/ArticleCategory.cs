﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Crawler.Respository
{
    [Table("ArticleCategory")]
    public class ArticleCategory : ModelEntity
    {
        public string Title { get; set; }

        public string Keyword { get; set; }
        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        //public string Content { get; set; }


        public string ParentId { get; set; }


    }





    /// <summary>
    /// 系统管理Model基类
    /// </summary>
    public class ModelEntity : CreatorEntity
    {
        public int? SortCode { get; set; }

        public bool? DeleteMark { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }

        public DateTime? LastModifyTime { get; set; }
        public string LastModifyUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }

    }


    public class CreatorEntity : KeyEntity
    {
        public virtual DateTime? CreatorTime { get; set; }
        public string CreatorUserId { get; set; }
    }

    public class KeyEntity
    {
        [Key]
        public virtual string Id { get; set; }
    }


}
