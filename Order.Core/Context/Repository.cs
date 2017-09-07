using Order.Core.Entity;
using Order.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Order.Core.Context
{
    public class Repository : IRepository<User, Entity.Order>, IDisposable
    {
        ~Repository()
        {
            Dispose(false);
        }

        private OrderDB _context;
        public IEnumerable<User> GetContext
        {
            get
            {
                var result = _context.User.ToList();
                result.ForEach(x => x.Orders = (_context.Order.Where(y => y.UserId == x.Id)));

                return result;
            }
        }

        public Int32 Save(User savedObject)
        {
            if (savedObject != null)
            {
                if (savedObject.Id == 0)
                {
                    _context.User.Add(savedObject);
                }
                else
                {
                    User tmpUser = _context.User.FirstOrDefault(x => x.Id == savedObject.Id);
                    tmpUser = savedObject;
                }
                return _context.SaveChanges();
            }
            return -1;
        }

        public Int32 Delete(User deletedObject)
        {
            if (deletedObject != null)
            {
                User tmpUser = _context.User.FirstOrDefault(x => x.Id == deletedObject.Id);
                List<Entity.Order> tmpOrders = _context.Order.Where(x => x.UserId == deletedObject.Id).ToList();
                _context.Order.RemoveRange(tmpOrders);
                _context.User.Remove(tmpUser);
                return _context.SaveChanges();
            }
            return -1;
        }

        public Int32 Save(Entity.Order savedObject)
        {
            if (savedObject != null)
            {
                if (savedObject.Id == 0)
                {
                    _context.Order.Add(savedObject);
                }
                else
                {
                    Entity.Order tmpOrder = _context.Order.FirstOrDefault(x => x.Id == savedObject.Id);
                    tmpOrder = savedObject;
                }
                return _context.SaveChanges();
            }
            return -1;
        }

        public Int32 Delete(Entity.Order deletedObject)
        {
            if (deletedObject != null)
            {
                Entity.Order tmpOrder = _context.Order.FirstOrDefault(x => x.Id == deletedObject.Id);
                _context.Order.Remove(tmpOrder);
                return _context.SaveChanges();
            }
            return -1;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }

        public Repository()
        {
            _context = new OrderDB();
        }
    }
}