using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartMeal.Data.Data;

namespace SmartMeal.Data
{
    class MyDbContextFactory : IDesignTimeDbContextFactory<SmartMealContext>
    {

        private string GetConnectionString()
        {
            DotNetEnv.Env.Load("../.env");
            string host = DotNetEnv.Env.GetString("POSTGRES_HOST");
            string port = DotNetEnv.Env.GetString("POSTGRES_PORT");
            string db = DotNetEnv.Env.GetString("POSTGRES_DB");
            string user = DotNetEnv.Env.GetString("POSTGRES_USER");
            string password = DotNetEnv.Env.GetString("POSTGRES_PASSWORD");
            string connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password};";

            return connectionString;
        }

        public SmartMealContext CreateDbContext(string[] args)
        {
            var connectionString = GetConnectionString();
            var builder = new DbContextOptionsBuilder<SmartMealContext>();
            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("SmartMeal.Migrations"));

            return new SmartMealContext(builder.Options);
        }
    }
}
