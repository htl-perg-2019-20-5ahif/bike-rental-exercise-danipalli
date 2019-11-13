using BikeRentalServiceWebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalServiceWebAPI.Data
{
    public class BikeRentalDbContext : DbContext
    {
        public BikeRentalDbContext(DbContextOptions<BikeRentalDbContext> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Bike> Bikes { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Customer>()
            //    .Property(c => c.FirstName)
            //    .IsRequired();
        }
    }
}
