using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SmartMeal.Api;
using SmartMeal.Data.Data;
using SmartMeal.Models.Models;
using Xunit;

namespace SmartMeal.Tests.FunctionalTests
{
    public class RegisterUser : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public RegisterUser(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions());
        }

        public async Task ShouldRegisterUser()
        {

            //Arrange
            var data = new {Email = "test@test.pl", Password = "test"};

            // Act
            var response = await _client.PostAsync("/api/Account/register", new JsonContent(data));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async void UserAlreadyExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            using (var context = new AppDbContext(options))
            {
                User user = new User()
                {
                    Id = 1,
                    Email = "test@test.pl"
                };
                context.Users.Add(user);
                context.SaveChanges();

            }
            using (var context = new AppDbContext(options))
            {
                var data = new { Email = "test@test.pl", Password = "test" };

                var response = await _client.PostAsync("/api/Account/register", new JsonContent(data));

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


            }


        }
    }
}
