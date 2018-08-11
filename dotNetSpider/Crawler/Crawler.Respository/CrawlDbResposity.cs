using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Respository
{
    public interface ICrawlDbResposity<TEntity> where TEntity : ModelEntity, new()
    {
        List<Article> GetArticles();
        void AddCategory(ArticleCategory entity);

        void Add(List<TEntity> entitys);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        void AddBetch(List<TEntity> entities, int groupNum = 50);

        IQueryable<TResult> Get<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
string includeProperties = "");
    }


    public class CrawlDbResposity<TEntity> : ICrawlDbResposity<TEntity> where TEntity:ModelEntity,new()
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


        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _CrawlDbContext.Set<TEntity>().Any(predicate);
        }


        public IQueryable<TResult> Get<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
string includeProperties = "")
        {
            var query = _CrawlDbContext.Set<TEntity>().AsTracking();
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Where(predicate).Select(select);
        }

        public void AddCategory(ArticleCategory entity)
        {
            if (_CrawlDbContext.Set<ArticleCategory>().Any(w => w.Id == entity.Id || w.Title == entity.Title))
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

        public void Add(List<TEntity> entitys)
        {
            try
            {
                entitys.ForEach(entity =>
                {
                    _CrawlDbContext.Entry(entity).State = EntityState.Added;
                });
                _CrawlDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="groupNum">默认50个批量提交一次</param>
        public void AddBetch(List<TEntity> entities, int groupNum = 50)
        {
            if (_CrawlDbContext.Database.GetDbConnection().State != ConnectionState.Open)
            {
                _CrawlDbContext.Database.OpenConnection(); //打开Connection连接
            }

            //调用BulkInsert方法,将entitys集合数据批量插入到数据库的tolocation表中
            using (var bulkCopy = new SqlBulkCopy((SqlConnection)_CrawlDbContext.Database.GetDbConnection()))
            {
                //table 表
                var tableAttribute = (TableAttribute)typeof(TEntity).GetCustomAttribute(typeof(TableAttribute));
                bulkCopy.DestinationTableName = tableAttribute.Name;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(TEntity))
                    .Cast<PropertyDescriptor>()
                    .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                    .ToArray();

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }


                bulkCopy.BatchSize = groupNum;

                var values = new object[props.Length];
                foreach (var item in entities)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }

            if (_CrawlDbContext.Database.GetDbConnection().State != ConnectionState.Closed)
            {
                _CrawlDbContext.Database.CloseConnection(); //关闭Connection连接
            }

        }


    }




}
