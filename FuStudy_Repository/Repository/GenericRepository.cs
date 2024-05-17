using Microsoft.EntityFrameworkCore;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository.Interface;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;

namespace FuStudy_Repository.Repository
{

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal MyDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(MyDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public Task<TEntity> Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetByFilterAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(long id)
        {
            return dbSet.Find(id);
        }

        public Task<TEntity> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll()
        {

            return dbSet.ToList();
        }
    }
}
