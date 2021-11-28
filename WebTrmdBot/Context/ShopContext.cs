using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TegBotTrmd.Entity;

namespace TegBotTrmd.Context
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options)
                 : base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                        .HasMany(x=>x.Products).WithOne(xx=>xx.Order);
            modelBuilder.Entity<Order>()
                        .HasOne(x => x.Customer).WithMany(xx=>xx.Orders);

            modelBuilder.Entity<Product>()
                       .HasOne(x => x.Order).WithMany(xx => xx.Products);

            modelBuilder.Entity<User>()
                       .HasMany(x => x.Orders).WithOne(xx => xx.Customer);

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
