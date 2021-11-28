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
    public class ProductRepository : IProductRepository
    {
        protected readonly ShopContext _context;
        public ProductRepository(ShopContext context)
        {
            _context = context;
        }

        public void Create(Product item)
        {
            return;
        }

        public void Delete(int id)
        {
            var tmp = _context.Products.FirstOrDefault(x => x.Id == id);
            if (tmp!=null)
            {
                _context.Products.Remove(tmp);
            }
        }

        public void Dispose()
        {
            return;
        }

        public Product GetProduct(int id)
        {
            return null;
        }
        public Product GetProductByNameAndPrice(string name,decimal price)
        {
            var tmp = _context.Products.Include(x=>x.Order).FirstOrDefault(x => x.Name == name && x.Price == price);
            return tmp;
        }

        public IEnumerable<Product> GetProductsList()
        {
            var tmp = _context.Products.ToList();
            return tmp;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Product item)
        {
            return;
        }
    }
}
