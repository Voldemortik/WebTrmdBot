using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TegBotTrmd.Entity
{
    public class User
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public List<Order> Orders { get; set; }
        public decimal Balance { get; set; }
    }
}
