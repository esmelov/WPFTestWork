using Order.Core.Entity;
using System;
using System.Collections.Generic;

namespace Order.Core.Interfaces
{
    public interface IRepository<T, T1> where T: User
                                        where T1: Entity.Order
    {
        IEnumerable<T> GetContext { get; }
        Int32 Save(T savedObject);
        Int32 Delete(T deletedObject);
        Int32 Save(T1 savedObject);
        Int32 Delete(T1 deletedObject);
    }
}