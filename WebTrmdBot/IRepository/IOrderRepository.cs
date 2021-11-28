using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TegBotTrmd.Entity;

namespace TegBotTrmd.IRepository
{
    public interface IOrderRepository : IDisposable
    {
        IEnumerable<Order> GetOrderListByUser(int Id); // получение всех объектов
        Order GetOrder(int id); // получение одного объекта по id
        void Create(Order item); // создание объекта
        void Update(Order item); // обновление объекта
        void Delete(int id); // удаление объекта по id
        void Save();  // сохранение изменений
    }
}
