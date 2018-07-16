using System;
using System.Linq;

namespace Crawler
{
   
    class Program
    {
        static void Main(string[] args)
        {
          
            try
            {
                CrawlHelper.Run();

                //Console.WriteLine("Hello World!");
                //var datas = typeof(CrawlHelper).GetProperties().Select(u => new { name = 1, value = 1 }).ToList();

                //var genType = datas.GetType().GenericTypeArguments[-1];

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.TargetSite +"\n"+ex.Message);
            }




        }
    }
}
