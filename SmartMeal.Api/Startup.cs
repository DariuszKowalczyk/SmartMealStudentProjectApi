using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMeal.Data.Data;
using SmartMeal.Data.Repository;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models.Models;
using SmartMeal.Service.Interfaces;
using SmartMeal.Service.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartMeal.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private string GetConnectionString()
        {
            string solution_path = Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory());
            string env_path = $"{solution_path}/.env";
            if (File.Exists(env_path))
            {
                DotNetEnv.Env.Load(env_path);
            }
            string host = DotNetEnv.Env.GetString("POSTGRES_HOST");
            string port = DotNetEnv.Env.GetString("POSTGRES_PORT");
            string db = DotNetEnv.Env.GetString("POSTGRES_DB");
            string user = DotNetEnv.Env.GetString("POSTGRES_USER");
            string password = DotNetEnv.Env.GetString("POSTGRES_PASSWORD");
            string connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password};";

            return connectionString;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetConnectionString();
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
                connectionString, b => b.MigrationsAssembly("SmartMeal.Data")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Smart Meal", Version = "v1" });
            });

            // Add services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<DbContext, AppDbContext>();

            //Add repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Meal V1");
            });

//            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
//            {
//                scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
//            }

            app.UseMvc();
            

        }
    }
}
