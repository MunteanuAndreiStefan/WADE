using FakeNewsDetectionCache.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Interfaces
{
    public interface ICrudService<T>
      where T : Entity, new()
    {

        Task<List<T>> GetAll();

        Task<List<T>> GetByFilter(Expression<Func<T, bool>> filter);

        Task<Guid> Add(T entity);

        Task<Guid> Update(T entity);

        Task<bool> Delete(T entity);

        Task<IQueryable<T>> GetAsQueriable();
    }
}
