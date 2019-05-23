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

        [Fact]
        public async void should_delete_product()
        {
            var product = new Product();
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(product);
            _productRepository.Setup(x => x.RemoveElement(It.IsAny<Product>()))
                .ReturnsAsync(true);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();

            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.DeleteProductAsync(5);

            Assert.False(response.IsError);
        }

        [Fact]
        public async void should_given_error_when_product_not_delete()
        {
            var product = new Product();
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(product);
            _productRepository.Setup(x => x.RemoveElement(It.IsAny<Product>()))
                .ReturnsAsync(false);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();

            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.DeleteProductAsync(5);

            Assert.True(response.IsError);
            Assert.Equal(Error.ProductErrorWhenDelete, response.Errors[0]);
        }

        [Fact]
        public async void should_return_list_of_product()
        {
            var productList = new List<Product>()
            {
                new Product()
                {
                    Name = "test1"
                },
                new Product()
                {
                    Name = "test2"
                },

            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Product, object>>>()))
                .ReturnsAsync(productList);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();

            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.GetProducts();

            Assert.False(response.IsError);
            Assert.True(response.Data.Count == 2);
        }
        [Fact]
        public async void should_return_empty_list_of_product()
        {
            var productList = new List<Product>();
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Product, object>>>()))
                .ReturnsAsync(productList);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();

            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.GetProducts();

            Assert.False(response.IsError);
            Assert.True(response.Data.Count == 0);
        }

        [Fact]
        public async void should_product_by_id_with_photo()
        {
            var photo = new Photo()
            {
                Id = 2,
                Filename = "test.jpg"
            };
            var product = new Product()
            {
                Id = 1,
                Name = "test1",
                Description = "Opis",
                Image = photo
            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Product, object>>>()))
                .ReturnsAsync(product);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.GetProductById(product.Id);

            Assert.False(response.IsError);
            Assert.Equal(product.Id, response.Data.Id);
            Assert.Equal(product.Description, response.Data.Description);
            Assert.Equal(product.Image.Filename, response.Data.ImagePath);
        }

        [Fact]
        public async void should_given_product_not_found()
        {
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Product, object>>>()))
                .ReturnsAsync((Product)null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.GetProductById(1);

            Assert.True(response.IsError);
            Assert.Equal(Error.ProductDoesntExist, response.Errors[0]);
        }

        [Fact]
        public async void should_update_product_without_photo()
        {
            var photo = new Photo()
            {
                Id = 1,
                Filename = "test.jpg"
            };
            var productBindingModel = new ProductBindingModel()
            {
                Name = "Nowa nazwa",
                Description = "Nowy Opis",
                ImagePath = "newTest.jpg"
            };
            var product = new Product()
            {
                Id = 1,
                Name = "Nazwa",
                Description = "Opis",
                Image = photo
            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(product);
            _productRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.UpdateProductAsync(productBindingModel, product.Id);

            Assert.False(response.IsError);
            Assert.Equal(productBindingModel.Name, response.Data.Name);
            Assert.Equal(productBindingModel.Description, response.Data.Description);
        }

        [Fact]
        public async void should_given_update_product_not_found()
        {
            var productBindingModel = new ProductBindingModel()
            {
                Name = "Nowa nazwa",
                Description = "Nowy Opis",
                ImagePath = "newTest.jpg"
            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Product)null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.UpdateProductAsync(productBindingModel, 1);

            Assert.True(response.IsError);
            Assert.Equal(Error.ProductDoesntExist, response.Errors[0]);
        }

        [Fact]
        public async void should_given_update_product_error_when_update()
        {
            var productBindingModel = new ProductBindingModel()
            {
                Name = "Nowa nazwa",
                Description = "Nowy Opis",
                ImagePath = "newTest.jpg"
            };
            var photo = new Photo()
            {
                Id = 1,
                Filename = "test.jpg"
            };
            var product = new Product()
            {
                Id = 1,
                Name = "Nazwa",
                Description = "Opis",
                Image = photo
            };
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(product);
            _productRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(false);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            var productService = new ProductService(_productRepository.Object, _userRepository.Object, _photoRepository.Object);

            var response = await productService.UpdateProductAsync(productBindingModel, 1);

            Assert.True(response.IsError);
            Assert.Equal(Error.ProductErrorWhenUpdate, response.Errors[0]);
        }
    }
}
