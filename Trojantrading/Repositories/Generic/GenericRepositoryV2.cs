using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Trojantrading.Repositories.Generic
{
    public class GenericRepositoryV2<TEntity> : ReadOnlyRelationshipRepositoryV2<TEntity>, IRepositoryV2<TEntity> where TEntity : class
    {

        public GenericRepositoryV2(TrojantradingDbContext context) : base(context) { }

        public void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            var entry = _context.Entry(entity);

            if (entry != null && entry.State != EntityState.Detached)
                entity = (TEntity)entry.Entity;
            else
                if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public void DeleteRange(List<TEntity> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public virtual Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public virtual void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void CreateRange(List<TEntity> entity)
        {
            _dbSet.AddRange(entity);
        }

        public virtual void UpdateRange(List<TEntity> entity)
        {
            _dbSet.UpdateRange(entity);
        }

        public virtual void Update(
            TEntity entity,
            Expression<Func<TEntity, object[]>> excludeProperties = null,
            Expression<Func<TEntity, object[]>> includeProperties = null)
        {
            var entry = _context.Entry(entity);
            if (entry != null && entry.State != EntityState.Detached)
            {
                _dbSet.Update(entity);
                entity = (TEntity)entry.Entity;
            }
            else
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = includeProperties != null ? EntityState.Unchanged : EntityState.Modified;
            }

            StringCollection strCollection = new StringCollection();
            NewArrayExpression array = null;

            if (excludeProperties != null)
                array = excludeProperties.Body as NewArrayExpression;
            else
                if (includeProperties != null)
                array = includeProperties?.Body as NewArrayExpression;
            if (array != null)
                foreach (var expression in array.Expressions)
                {
                    string propertyName;
                    if (expression is UnaryExpression)
                    {
                        propertyName = ((MemberExpression)((UnaryExpression)expression).Operand).Member.Name;
                    }
                    else
                    {
                        propertyName = ((MemberExpression)expression).Member.Name;
                    }

                    strCollection.Add(propertyName);
                }

            if (excludeProperties != null)
            {
                foreach (var propName in strCollection)
                    _context.Entry(entity).Property(propName).IsModified = false;
            }
            else if (includeProperties != null)
            {
                foreach (var propName in strCollection)
                    _context.Entry(entity).Property(propName).IsModified = true;
            }
        }
    }
}
