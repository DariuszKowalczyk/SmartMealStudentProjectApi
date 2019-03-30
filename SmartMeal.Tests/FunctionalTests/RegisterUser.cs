using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using SmartMeal.Api;
using Xunit;

namespace SmartMeal.Tests.FunctionalTests
{
    public class RegisterUser : IClassFixture<CustomWeApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWeApplicationFactory<Startup> _factory;

        public RegisterUser(CustomWeApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions());
        }


    }
}
