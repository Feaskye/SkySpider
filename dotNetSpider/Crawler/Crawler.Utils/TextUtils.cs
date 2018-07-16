using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Utils
{
    public class TextUtils
    {
        private static string[] chars = new string[] { "\r", "\n", "\r\n", " ", "  ", "\"","'" };


        public static string ReplaceChar(string s)
        {
            foreach (var chr in chars)
            {
                s = s.Replace(chr, "");
            }
            return s;
        }

    }
}
