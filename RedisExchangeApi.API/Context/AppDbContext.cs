using Microsoft.EntityFrameworkCore;
using RedisExchangeApi.API.Entities;

namespace RedisExchangeApi.API.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Data

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Kalem", Price = 5 },
                new Product { Id = 2, Name = "Silgi", Price = 10 },
                new Product { Id = 3, Name = "Defter", Price = 15 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
