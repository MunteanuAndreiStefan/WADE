using FakeNewsDetectionCache.Entities;
using FakeNewsDetectionCache.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Service
{
    //[Log]
    public abstract class CrudService<T, TDbContext> : BaseService<TDbContext>
      , ICrudService<T>
      where T : Entity, new()
      where TDbContext : DbContext
    {
        protected DbSet<T> DbSet => DbContext.Set<T>();
        protected CrudService(TDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Guid> Add(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Added;
            await DbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> Delete(T entity)
        {
            if (entity == null)
                return false;

            DbContext.Entry(entity).State = EntityState.Deleted;
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<List<T>> GetByFilter(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
                return await DbSet.Where(filter).ToListAsync();
            return await DbSet.ToListAsync();
        }

        public async Task<Guid> Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IQueryable<T>> GetAsQueriable()
        {
            return DbSet.AsNoTracking().AsQueryable();
        }
    }
}
