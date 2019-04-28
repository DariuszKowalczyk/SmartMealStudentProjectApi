using System;
using System.Collections.Generic;
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
using SmartMeal.Service.Services;
using Xunit;

namespace SmartMeal.Tests.UnitTests
{
    public class TestRegisterUser
    {

        public TestRegisterUser()
        {

        }
        [Fact]
        public async void ShouldRegisterNewUser()
        {

            // Arrange
            var registerData = new RegisterDto()
            {
                Email = "test@test.pl",
                Password = "test"
            };
            IActionResult result = new OkResult();

            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.CreateAsync(It.IsAny<User>())).Returns(Task.FromResult(true));
            var accountService = new AccountService(userRepository.Object);
            var accountController = new AccountController(accountService);

            // Act
            var response = await accountController.Register(registerData);
            // Assert
            Assert.IsType<OkResult>(response); 
        }
        
        [Fact]
        public async void InvalidEmail()
        {
            // Arrange
            var repository = new Mock<IRepository<User>>();
            var service = new AccountService(repository.Object);
            var controller = new AccountController(service);
            controller.ModelState.AddModelError("error", "some error");
            // Act
            var response = await controller.Register(model: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}
