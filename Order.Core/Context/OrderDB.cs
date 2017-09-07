using Order.Core.Entity;
using System.Data.Entity;
using EntityOrder = Order.Core.Entity.Order;

namespace Order.Core.Context
{
    internal class OrderDB : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<EntityOrder> Order { get; set; }
    }
}