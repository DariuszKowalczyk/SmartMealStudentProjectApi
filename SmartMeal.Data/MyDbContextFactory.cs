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
            string connectionString = $"host={host};port={port};database={db};username={user};password={password};";

            return connectionString;
        }

        public SmartMealContext CreateDbContext(string[] args)
        {
            var connectionString = GetConnectionString();
            var builder = new DbContextOptionsBuilder<SmartMealContext>();
            builder.UseSqlServer(connectionString,
                optionsBuilder =>
                    optionsBuilder.MigrationsAssembly((typeof(SmartMealContext).GetTypeInfo().Assembly.GetName()
                        .Name)));

            return new SmartMealContext(builder.Options);
        }
    }
}
