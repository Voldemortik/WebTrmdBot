using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TegBotTrmd.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public User Customer { get; set; }
        public bool IsPaid { get; set; }
    }
}
