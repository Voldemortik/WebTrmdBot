using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TegBotTrmd.Context;
using TegBotTrmd.Entity;
using TegBotTrmd.IRepository;

namespace TegBotTrmd.Repository
{
    public class OrderRepository : IOrderRepository
    {
        protected readonly ShopContext _context;
        public OrderRepository(ShopContext context)
        {
            _context = context;
        }
        public void Create(Order item)
        {
            _context.Orders.Add(item);
        }

        public void Delete(int id)
        {
            return;
        }

        public void Dispose()
        {
            return;
        }

        public Order GetOrder(int id)
        {
            var tmp = _context.Orders.Where(x => x.Id == id).Include(x=>x.Products).Include(x => x.Customer).FirstOrDefault();
            return tmp;
        }

        public IEnumerable<Order> GetOrderListByUser(int id)
        {
            var tmp = _context.Orders.Where(x => x.Customer.Id == id).Include(x => x.Products).Include(x=>x.Customer).ToList();
            return tmp;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Order item)
        {
            return;
        }
    }
}
