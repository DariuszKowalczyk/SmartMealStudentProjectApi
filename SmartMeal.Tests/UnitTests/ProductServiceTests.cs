using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartMeal.Api.Controllers;
using SmartMeal.Data.Data;
using SmartMeal.Data.Repository;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service;
using SmartMeal.Service.Services;
using Xunit;

namespace SmartMeal.Tests.UnitTests
{
    public class ProductServiceTests
    {
        private Mock<IRepository<Product>> _productRepository { get; set; }

       public ProductServiceTests()
        {

        }
        [Fact]
        public async void should_create_new_product()
        {
            var product = new ProductDto()
            {
                Name = "test",
                Description = "DescTest"
            };
            var productService = new ProductService(_productRepository.Object);

        }
         
    }
}
