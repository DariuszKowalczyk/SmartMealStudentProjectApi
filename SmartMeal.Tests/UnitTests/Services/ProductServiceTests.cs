using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartMeal.Api.Controllers;
using SmartMeal.Data.Data;
using SmartMeal.Data.Repository;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service;
using SmartMeal.Service.Services;
using Xunit;

namespace SmartMeal.Tests.UnitTests
{
    public class ProductServiceTests
    {
        public ProductServiceTests()
        {
            AutoMapperConfig.Initialize();
        }

        [Fact]
        public async void should_create_new_product_without_photo()
        {
            var productBindingModel = new ProductBindingModel()
            {
                Name = "test",
                Description = "DescTest"
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.CreateAsync(It.IsAny<Product>()))
                .Returns(Task.FromResult(true));
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Photo) null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);

            
            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.CreateProductAsync(productBindingModel, user.Id);

            Assert.False(response.IsError);
            Assert.Equal(productBindingModel.Name, response.Data.Name);
            Assert.Equal(productBindingModel.Description, response.Data.Description);
            Assert.Null(response.Data.ImagePath);
            Assert.NotNull(response.Data.Id);
        }

        [Fact]
        public async void should_create_new_product_with_photo()
        {
            var productBindingModel = new ProductBindingModel()
            {
                Name = "test",
                Description = "DescTest",
                ImagePath = "test.jpg"
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var photo = new Photo()
            {
                Id = 1,
                Filename = "test.jpg"

            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.CreateAsync(It.IsAny<Product>())).Returns(Task.FromResult(true));
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(photo);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.CreateProductAsync(productBindingModel, user.Id);

            Assert.False(response.IsError);
            Assert.NotNull(response.Data.Id);
            Assert.Equal(productBindingModel.Name, response.Data.Name);
            Assert.Equal(productBindingModel.Description, response.Data.Description);
            Assert.Equal(productBindingModel.ImagePath, response.Data.ImagePath);
        }

        [Fact]
        public async void should_create_given_product_name_already_exist()
        {
            var productBindingModel = new ProductBindingModel()
            {
                Name = "test",
                Description = "DescTest",
                ImagePath = "test.jpg"
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns(Task.FromResult(true));
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Photo)null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await  productService.CreateProductAsync(productBindingModel, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.ProductExist, response.Errors[0]);
        }

        [Fact]
        public async void should_delete_given_product_not_found()
        {
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Product)null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();

            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.DeleteProductAsync(0);

            Assert.True(response.IsError);
            Assert.Equal(Error.ProductDoesntExist, response.Errors[0]);
        }

//        [Fact]
//        public async void should_delete_product()
//        {
//            var product = new Product();
//            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
//            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>())).ReturnsAsync(product);
//            _productRepository.Setup(x => x.RemoveElement(It.IsAny<Product>())).Returns(Task.FromResult(true));
//            var productService = new ProductService(_productRepository.Object);
//
//            var response = await productService.DeleteProductAsync(5);
//
//            Assert.False(response.IsError);
//        }
    }
}
