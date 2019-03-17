using Microsoft.EntityFrameworkCore;
using SmartMeal.Models.Models;

namespace SmartMeal.Data.Data
{
    public class SmartMealContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SmartMealContext(DbContextOptions<SmartMealContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
