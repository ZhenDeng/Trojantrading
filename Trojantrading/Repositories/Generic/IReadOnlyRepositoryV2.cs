using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trojantrading.Models;

namespace Trojantrading.Repositories.Generic
{
    public interface IReadOnlyRepositoryV2<TEntity> where TEntity : class
    {
        List<TEntity> All(IPagination pagination = null);
        Task<List<TEntity>> AllAsync(IPagination pagination = null);

        List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, IPagination pagination = null);
        Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, IPagination pagination = null);

        int GetCount(Expression<Func<TEntity, bool>> predicate);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        bool Exists(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FindByKey(object id);
        Task<TEntity> FindByKeyAsync(object id);
        IQueryable<TEntity> Queryable { get; }
    }
}
