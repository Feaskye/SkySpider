using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Utils.Models
{

    public class BaseTag
    {
        public string Text { get; set; }
    }


    /// <summary>
    /// a 标签
    /// </summary>
    public class Anchor: BaseTag
    {
        public string Href { get; set; }
    }
}
