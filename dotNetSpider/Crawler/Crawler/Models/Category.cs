using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Models
{
    public class Category
    {
        public string Name { get; set; }

        public string Url { get; set; }


        public List<Category> Childs { get; set; }
    }
}
