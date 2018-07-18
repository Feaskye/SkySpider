using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Utils
{
    public static class TextUtils
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

        public static int ToInt(this string s)
        {
            int p = 0;
            int.TryParse(ReplaceChar(s),out p);
            return p;
        }






    }
}
