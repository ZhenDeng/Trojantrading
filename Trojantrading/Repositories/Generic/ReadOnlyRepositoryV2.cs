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
    public class ReadOnlyRepositoryV2<TEntity> : IReadOnlyRepositoryV2<TEntity> where TEntity : class
    {
        protected TrojantradingDbContext _context;
        protected virtual DbSet<TEntity> _dbSet { get; }
        public ReadOnlyRepositoryV2(TrojantradingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public List<TEntity> All(IPagination pagination = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            return query.Paging(pagination).ToList();
        }

        public Task<List<TEntity>> AllAsync(IPagination pagination = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            return query.Paging(pagination).ToListAsync();
        }


        public List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, IPagination pagination = null)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);

            return query.Paging(pagination).ToList();
        }

        public Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, IPagination pagination = null)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);

            return query.Paging(pagination).ToListAsync();
        }

        public int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);

            return query.Count();
        }

        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);

            return query.CountAsync();
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);

            return query.Any();
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);

            return query.AnyAsync();
        }

        public TEntity FindByKey(object id)
        {
            return _dbSet.Find(id);
        }

        public Task<TEntity> FindByKeyAsync(object id)
        {
            return _dbSet.FindAsync(id);
        }

        public IQueryable<TEntity> Queryable
        {
            get
            {
                return _dbSet.AsNoTracking();
            }
        }

    }
}
