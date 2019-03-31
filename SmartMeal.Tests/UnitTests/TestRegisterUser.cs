using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartMeal.Api.Controllers;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Services;
using Xunit;

namespace SmartMeal.Tests.UnitTests
{
    public class TestRegisterUser
    {
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
            userRepository.Setup(x => x.CreateAsync(new User())).Returns(Task.FromResult(true));
            var accountService = new AccountService(userRepository.Object);
            var AccountController = new AccountController(accountService);

            // Act
            var response = await AccountController.Register(registerData);
            // Assert
            Assert.IsType<OkResult>(response);
            
        }
    }
}
