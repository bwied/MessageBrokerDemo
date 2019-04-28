using System;
using System.Collections.Generic;

namespace OrderService
{
  public interface IRepository<T> : IDisposable
  {
    T Get(Guid id);

    void Save(T aggregate);
  }
}
