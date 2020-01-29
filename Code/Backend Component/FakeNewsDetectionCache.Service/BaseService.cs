using FakeNewsDetectionCache.Entities;
using FakeNewsDetectionCache.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Service
{
  public abstract class BaseService<TDbContext>
    where TDbContext:DbContext
  {
    protected TDbContext DbContext;

    protected BaseService(TDbContext dbContext)
    {
      DbContext = dbContext;
    }
  }
}
