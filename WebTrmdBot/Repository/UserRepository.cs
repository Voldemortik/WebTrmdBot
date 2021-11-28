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
    public class UserRepository : IUserRepository
    {
        protected readonly ShopContext _context;
        public UserRepository(ShopContext context)
        {
            _context = context;
        }
        public void Create(User item)
        {
            _context.Users.Add(item);
            Save();
        }

        public void Delete(int id)
        {
            return;
        }

        public void Dispose()
        {
            return;
        }

        public User GetUser(int id)
        {
            return null;
        }

        public User GetUserByChatId(long id)
        {
            var tmp = _context.Users.FirstOrDefault(x => x.ChatId == id);
            return tmp;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User item)
        {
            return;
        }
    }
}
