using System;
using System.Collections.Generic;
using System.Text;
using SmartMeal.Service;
using Xunit;

namespace SmartMeal.Tests.UnitTests.Services
{
    public class RecipeServiceTests
    {
        public RecipeServiceTests()
        {
            AutoMapperConfig.Initialize();
        }

        [Fact]
        public async void should_create_new_recipe_without_photo()
        {
            Assert.True(true);
        }
    }
}
