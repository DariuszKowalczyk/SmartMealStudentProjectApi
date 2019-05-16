using Microsoft.EntityFrameworkCore;
using SmartMeal.Models.Models;

namespace SmartMeal.Data.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredient");
            modelBuilder.Entity<Recipe>().ToTable("Recipe");

            modelBuilder.Entity<Ingredient>().HasOne(i => i.Recipe).WithMany(r => r.Ingredients);
            modelBuilder.Entity<Ingredient>().HasOne(i => i.Product).WithMany(p => p.Ingredients);



        }
    }
}
