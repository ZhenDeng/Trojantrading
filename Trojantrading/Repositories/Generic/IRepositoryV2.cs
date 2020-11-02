using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Trojantrading.Repositories.Generic
{
    public interface IRepositoryV2<TEntity> : IReadOnlyRelationshipRepositoryV2<TEntity> where TEntity : class
    {
        void Delete(object id);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entity);
        void Create(TEntity entity);
        void CreateRange(List<TEntity> entity);
        void Update(
            TEntity entity,
            Expression<Func<TEntity, object[]>> excludeProperties = null,
            Expression<Func<TEntity, object[]>> includeProperties = null);
        void UpdateRange(List<TEntity> entity);
        Task SaveChangesAsync();
    }
}
