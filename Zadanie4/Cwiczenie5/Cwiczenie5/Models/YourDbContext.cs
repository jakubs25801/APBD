using Microsoft.EntityFrameworkCore;

namespace Cwiczenie5.Data.Models
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Client_Trip> Client_Trip { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Country_Trip> Country_Trip { get; set; }
        public DbSet<Trip> Trip { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji i kluczy obcych
        }
    }
}