using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TegBotTrmd.Entity;

namespace TegBotTrmd.IRepository
{
    public interface IProductRepository : IDisposable
    {
        IEnumerable<Product> GetProductsList(); // получение всех объектов
        Product GetProduct(int id); // получение одного объекта по id
        Product GetProductByNameAndPrice(string name, decimal price);
        void Create(Product item); // создание объекта
        void Update(Product item); // обновление объекта
        void Delete(int id); // удаление объекта по id
        void Save();  // сохранение изменений
    }
}
