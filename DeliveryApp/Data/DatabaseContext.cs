using DeliveryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {

        }

        public DbSet<User> Users {get; set;}
        public DbSet<TransportedObject> TransportedObjects {get; set;}
        public DbSet<Delivery> Deliveries {get; set;}

        public DbSet<Location> Locations {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(user => user.Role)
                .HasConversion<string>();
                
            modelBuilder.Entity<Delivery>()
                .Property(d => d.Status)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
        
    }
}