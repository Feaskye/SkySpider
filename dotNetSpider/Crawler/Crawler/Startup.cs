using Crawler.Respository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler
{
    class Startup
    {

        public void Start()
        {
            var serviceProvider =ConfigureProvider(new ServiceCollection());

            CrawlHelper.Run(serviceProvider);
        }

        public IConfiguration Configuration { get; }

        public ServiceProvider ConfigureProvider(IServiceCollection services)
        {
            //services.AddLogging(logging => {
            //    logging.AddConfiguration(Configuration.GetSection("Logging"));
            //    logging.AddDebug();
            //});

            services.AddDbContext<CrawlDbContext>(options =>
                                options.UseSqlServer("#")); //配置mariadb连接字符串}

            services.AddScoped<ICrawlDbContext, CrawlDbContext>();
            services.AddScoped<ICrawlDbResposity<ArticleCategory>, CrawlDbResposity<ArticleCategory>>();
            services.AddScoped<ICrawlDbResposity<Article>, CrawlDbResposity<Article>>();
            return services.BuildServiceProvider();
        }
    }
}
