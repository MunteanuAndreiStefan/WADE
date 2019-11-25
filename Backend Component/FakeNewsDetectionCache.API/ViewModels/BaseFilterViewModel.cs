using FakeNewsDetectionCache.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.API.ViewModels
{
    public abstract class BaseFilterViewModel<T>
      where T : Entity, new()
    {
        public abstract Expression<Func<T, bool>> GetFilter();

        public abstract Task<List<T>> ApplyFilter(IQueryable<T> entities);
    }
}
