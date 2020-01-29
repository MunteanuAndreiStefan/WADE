using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNewsDetectionCache.Entities
{
  public abstract class Entity
  {
        public Guid Id { get; set; } = new Guid();
  }
}
