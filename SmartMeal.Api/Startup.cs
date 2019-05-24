using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SmartMeal.Data.Data;
using SmartMeal.Data.Repository;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Service;
using SmartMeal.Service.Interfaces;
using SmartMeal.Service.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Text;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;


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
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

            });

            AutoMapperConfig.Initialize();

            services.AddCors();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add services
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITimetableService, TimetableService>();
            services.AddScoped<IIgredientService, IgredientService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IFacebookService, FacebookService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            //Add repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var wkHtmlToPdfPath = Path.Combine(env.ContentRootPath, $"libwkhtmltox");

            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(wkHtmlToPdfPath);


            app.UseCors(builder =>builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());

            string ImagePath = Path.Combine(env.ContentRootPath, "Images");
            if (!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(ImagePath),
                RequestPath = "/static/images"
            });

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

            app.UseAuthentication();

            app.UseMvc();
            

        }
    }
}
