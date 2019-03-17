using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SmartMeal.Data.Data
{
    public class SmartMealContext : DbContext
    {
        public SmartMealContext(DbContextOptions<SmartMealContext> options) : base(options)
        { 
        }
    }
}
