using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using SmartMeal.Api;
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

        [Fact]
        public async Task ShouldRegisterUser()
        {

            //Arrange
            var data = new {Email = "test@test.pl", Password = "test"};

            // Act
            var response = await _client.PostAsync("/api/Account/register", new JsonContent(data));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
