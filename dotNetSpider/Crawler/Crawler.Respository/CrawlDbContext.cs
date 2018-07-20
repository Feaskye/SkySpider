using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Respository
{
    public interface ICrawlDbContext : IDbContext
    {

    }



    //http://www.cnblogs.com/ziye/p/7562889.html
    public class CrawlDbContext : DbContext, ICrawlDbContext
    {
        //Microsoft.EntityFrameworkCore.DbContext;

        public CrawlDbContext(DbContextOptions<CrawlDbContext> options) : base(options)
        {

        }



        public DbSet<ArticleCategory> Address { get; set; }

        public DbSet<Article> Articles { get; set; }





    }


    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>()
             where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        int SaveChanges();

        DatabaseFacade Database { get; }
    }

}
