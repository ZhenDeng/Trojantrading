using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trojantrading.Models;
using Trojantrading.Utilities;

namespace Trojantrading.Repositories.Generic
{
    public class ReadOnlyRelationshipRepositoryV2<TEntity> : ReadOnlyRepositoryV2<TEntity>, IReadOnlyRelationshipRepositoryV2<TEntity> where TEntity : class
    {

        public ReadOnlyRelationshipRepositoryV2(TrojantradingDbContext context) : base(context) { }

        public Task<List<TEntity>> AllIncludeAsync(IPagination pagination = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAllIncluding(includeProperties);
            return query.Paging(pagination).ToListAsync();
        }
        public List<TEntity> FindByInclude(Expression<Func<TEntity, bool>> predicate, IPagination pagination = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAllIncluding(includeProperties).Where(predicate);
            return query.Paging(pagination).ToList();
        }

        public List<TEntity> AllInclude(IPagination pagination, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAllIncluding(includeProperties);
            return query.Paging(pagination).ToList();
        }

        //public IList<TResponse> GetFromStoredProcedure<TResponse>(string storedProcedureName, params object[] parameters) where TResponse : class, new()
        //{
        //    _context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        //    return _context.Database.SqlQuery<TResponse>(
        //              storedProcedureName, parameters).ToList();
        //}

        //public int UpdateFromStoredProcedure(string storedProcedureName, params object[] parameters)
        //{
        //    _context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        //    return _context.Database.ExecuteSqlCommand(storedProcedureName, parameters);
        //}

        public Task<List<TEntity>> FindByIncludeAsync(Expression<Func<TEntity, bool>> predicate, IPagination pagination = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAllIncluding(includeProperties).Where(predicate);
            return query.Paging(pagination).ToListAsync();
        }

        private IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _dbSet;

            return includeProperties.Aggregate
              (queryable, (current, includeProperty) => current.Include(includeProperty));
        }

    }
}
