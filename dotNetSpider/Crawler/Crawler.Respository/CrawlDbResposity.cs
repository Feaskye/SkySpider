using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Respository
{
    public interface ICrawlDbResposity
    {
        List<Article> GetArticles();
        void AddCategory(ArticleCategory entity);

        void AddArticle(Article entity);
    }


    public class CrawlDbResposity: ICrawlDbResposity
    {
        ICrawlDbContext _CrawlDbContext;
        public CrawlDbResposity(ICrawlDbContext crawlDbContext)
        {
            _CrawlDbContext = crawlDbContext;
        }

        public List<Article> GetArticles()
        {
            return _CrawlDbContext.Set<Article>().ToList();
        }

        public void AddCategory(ArticleCategory entity)
        {
            if (_CrawlDbContext.Set<ArticleCategory>().Any(w => w.Id == entity.Id))
            {
                return;
            }
            try
            {
                _CrawlDbContext.Entry(entity).State = EntityState.Added;
                _CrawlDbContext.SaveChanges();
            }
            catch { }
        }

        public void AddArticle(Article entity)
        {
            if (_CrawlDbContext.Set<Article>().Any(w => w.Id == entity.Id))
            {
                return;
            }
            try
            {
                _CrawlDbContext.Entry(entity).State = EntityState.Added;
                _CrawlDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }




}
