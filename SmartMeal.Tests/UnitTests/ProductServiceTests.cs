//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using SmartMeal.Api.Controllers;
//using SmartMeal.Data.Data;
//using SmartMeal.Data.Repository;
//using SmartMeal.Data.Repository.Interfaces;
//using SmartMeal.Models.BindingModels;
//using SmartMeal.Models.Models;
//using SmartMeal.Models.ModelsDto;
//using SmartMeal.Service;
//using SmartMeal.Service.Services;
//using Xunit;
//
//namespace SmartMeal.Tests.UnitTests
//{
//    public class ProductServiceTests
//    {
//        private Mock<IRepository<Product>> _productRepository { get; set; }
//
//
//       public ProductServiceTests()
//        {
//
//        }
//       
//        [Fact]
//        public async void should_create_new_product_without_photo()
//        {
//            var product = new ProductBindingModel()
//            {
//                Name = "test",
//                Description = "DescTest"
//            };
//            _productRepository = new Mock<IRepository<Product>>();
//            _productRepository.Setup(x => x.CreateAsync(It.IsAny<Product>())).Returns(Task.FromResult(true));
//            var productService = new ProductService(_productRepository.Object);
//            var response = await productService.CreateProductAsync(product);
//
//            Assert.False(response.IsError);
//        }
//
//        [Fact]
//        public async void should_create_new_product_with_photo()
//        {
//            var product = new ProductBindingModel()
//            {
//                Name = "test",
//                Description = "DescTest",
//                ImagePath = "test.jpg"
//            };
//            _productRepository = new Mock<IRepository<Product>>();
//            _productRepository.Setup(x => x.CreateAsync(It.IsAny<Product>())).Returns(Task.FromResult(true));
//            var productService = new ProductService(_productRepository.Object);
//            var response = await productService.CreateProductAsync(product);
//
//            Assert.False(response.IsError);
//        }
//
//        [Fact]
//        public async void should_create_given_product_name_already_exist()
//        {
//            var product = new ProductBindingModel()
//            {
//                Name = "test",
//                Description = "DescTest"
//            };
//            _productRepository = new Mock<IRepository<Product>>();
//            _productRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Product, bool>>>())).Returns(Task.FromResult(true));
//            var productService = new ProductService(_productRepository.Object);
//            var response = await  productService.CreateProductAsync(product);
//            Assert.False(response.IsError);
//        }
//
//        [Fact]
//        public async void should_delete_given_product_not_found()
//        {
//            
//            _productRepository = new Mock<IRepository<Product>>();
//            _productRepository.Setup(x => x.RemoveElement(It.IsAny<Product>())).Returns(Task.FromResult(false));
//            var productService = new ProductService(_productRepository.Object);
//
//            var response = await productService.DeleteProductAsync(0);
//
//            Assert.True(response.IsError);
//        }
//         
//    }
//}
