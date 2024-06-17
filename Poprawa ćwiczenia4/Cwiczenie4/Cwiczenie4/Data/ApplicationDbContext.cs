using Microsoft.EntityFrameworkCore;
using Cwiczenie4.Models;
namespace Cwiczenie4.Data;
public class ApplicationDbContext : DbContext {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductWarehouse> ProductWarehouses { get; set; }
}
