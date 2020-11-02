using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trojantrading.Models;

namespace Trojantrading.Repositories.Generic
{
    public interface IReadOnlyRelationshipRepositoryV2<TEntity> : IReadOnlyRepositoryV2<TEntity> where TEntity : class
    {
        List<TEntity> AllInclude(IPagination pagination = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> AllIncludeAsync(IPagination pagination = null, params Expression<Func<TEntity, object>>[] includeProperties);

        List<TEntity> FindByInclude(Expression<Func<TEntity, bool>> predicate, IPagination pagination = null, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<List<TEntity>> FindByIncludeAsync(Expression<Func<TEntity, bool>> predicate, IPagination pagination = null, params Expression<Func<TEntity, object>>[] includeProperties);

        //IList<TResponse> GetFromStoredProcedure<TResponse>(string storedProcedureName, params object[] parameters) where TResponse : class, new();
    }
}
