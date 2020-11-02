using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trojantrading.Models;

namespace Trojantrading.Utilities
{
    public static class Utilities
    {
        public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(int id)
        {
            var item = Expression.Parameter(typeof(TEntity), "entity");
            var prop = Expression.Property(item, typeof(TEntity).Name + "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
            return lambda;
        }
        public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(Guid id)
        {
            var item = Expression.Parameter(typeof(TEntity), "entity");
            var prop = Expression.Property(item, typeof(TEntity).Name + "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
            return lambda;
        }
        public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(string id)
        {
            var item = Expression.Parameter(typeof(TEntity), "entity");
            var prop = Expression.Property(item, typeof(TEntity).Name + "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
            return lambda;
        }

        public static IQueryable<TEntity> Paginating<TEntity>(this IQueryable<TEntity> query, IPagination pagination) where TEntity : class
        {
            if (pagination == null || pagination.PageNumber == 0 || pagination.PageSize == 0)
                return query;

            query = query
                .Skip(pagination.PageSize * (pagination.PageNumber - 1))
                .Take(pagination.PageSize);

            return query;
        }

        public static async Task<TEntity[]> GetArrayAsync<TEntity>(this IQueryable<TEntity> query)
        {
            if (query is AsyncEnumerable)
                return await query.ToArrayAsync();
            else
                return query.ToArray();
        }
        public static async Task<List<TEntity>> GetListAsync<TEntity>(this IQueryable<TEntity> query)
        {
            if (query is AsyncEnumerable)
                return await query.ToListAsync();
            else
                return query.ToList();
        }

        public static async Task<TEntity> GetFirstOrDefaultAsync<TEntity>(this IQueryable<TEntity> query)
        {
            if (query is AsyncEnumerable)
                return await query.FirstOrDefaultAsync();
            else
                return query.FirstOrDefault();
        }
        public static async Task<TEntity> GetSingleOrDefaultAsync<TEntity>(this IQueryable<TEntity> query)
        {
            if (query is AsyncEnumerable)
                return await query.SingleOrDefaultAsync();
            else
                return query.SingleOrDefault();
        }

        public static IQueryable<TEntity> Including<TEntity>(
            this IQueryable<TEntity> query,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate
              (query, (current, includeProperty) => current.Including(includeProperty));

        }

        public static IQueryable<TEntity> Including<TEntity>(
           this IQueryable<TEntity> query,
           params string[] includePropertiesPath)
        {
            return includePropertiesPath.Aggregate
              (query, (current, includeProperty) => current.Including(includeProperty));
        }

        public static IQueryable<TEntity> Paging<TEntity>(
           this IQueryable<TEntity> query, IPagination pagination)
        {
            if (pagination == null || pagination.PageNumber <= 0 || pagination.PageSize <= 0)
                return query;

            query = query
                    .Skip(pagination.PageSize * (pagination.PageNumber - 1))
                    .Take(pagination.PageSize);

            return query;
        }

        public static IQueryable<TEntity> Paging<TEntity>(
           this IQueryable<TEntity> query, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return query;

            query = query
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize);

            return query;
        }

    }
}
