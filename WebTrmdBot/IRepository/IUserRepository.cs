using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TegBotTrmd.Entity;

namespace TegBotTrmd.IRepository
{
    public interface IUserRepository : IDisposable
    {
        User GetUser(int id); // получение одного объекта по id
        User GetUserByChatId(long id);
        void Create(User item); // создание объекта
        void Update(User item); // обновление объекта
        void Delete(int id); // удаление объекта по id
        void Save();  // сохранение изменений
    }
}
