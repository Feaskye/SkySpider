using Crawler.Respository;
using Microsoft.EntityFrameworkCore;
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

        public ServiceProvider ConfigureProvider(IServiceCollection services)
        {
            services.AddDbContext<CrawlDbContext>(options =>
                                options.UseSqlServer("Password=#feaskye888#;Persist Security Info=True;User ID=sa;Initial Catalog=SkyOilBase;Data Source=47.96.127.57,1433")); //配置mariadb连接字符串}

            services.AddScoped<ICrawlDbContext, CrawlDbContext>();
            services.AddScoped<ICrawlDbResposity, CrawlDbResposity>();
           return services.BuildServiceProvider();
        }
    }
}
