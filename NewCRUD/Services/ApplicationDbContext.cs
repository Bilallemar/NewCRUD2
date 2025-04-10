using Microsoft.EntityFrameworkCore;
using NewCRUD.Models;

namespace NewCRUD.Services
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
       public DbSet<Product> Products {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Lapstic", Brand = "hh", Category = "gg", Price = 44, Description = "llll", ImageFileName = "1.jpg" }
               
                );
        }
    }

}

