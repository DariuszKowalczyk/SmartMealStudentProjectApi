using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMeal.Data.Data;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartMeal.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DotNetEnv.Env.Load("../.env");
        }

        private string GetConnectionString()
        {
            string host = DotNetEnv.Env.GetString("POSTGRES_HOST");
            string port = DotNetEnv.Env.GetString("POSTGRES_PORT");
            string db = DotNetEnv.Env.GetString("POSTGRES_DB");
            string user = DotNetEnv.Env.GetString("POSTGRES_USER");
            string password = DotNetEnv.Env.GetString("POSTGRES_PASSWORD");
            string connectionString = $"host={host};port={port};database={db};username={user};password={password};";

            return connectionString;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetConnectionString();
            string migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddEntityFrameworkNpgsql().AddDbContext<SmartMealContext>(options => 
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationAssembly)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Smart Meal", Version = "v1" });
            });



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
            app.UseMvc();
            

        }
    }
}
