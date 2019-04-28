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
    public class AccountServiceTests
    {
        private Mock<IRepository<User>> _userRepository {get; set;}

        public AccountServiceTests()
        {
           _userRepository = new Mock<IRepository<User>>();
        }

        [Fact]
        public async void given_missing_password_should_return_false()
        {
            var registerData = new RegisterDto()
            {
                Email = "test@test.pl",
                Password = ""
            };
            var accountService = new AccountService(_userRepository.Object);
            var result = await accountService.CreateUserAsync(registerData);
            Assert.False(result);
        }

        [Fact]
        public async void given_user_already_exists_should_return_false()
        {
            var registerData = new RegisterDto()
            {
                Email = "test@test.pl",
                Password = ""
            };
            _userRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult(true));
            var accountService = new AccountService(_userRepository.Object);
            await Assert.ThrowsAsync<SmartMealException>(() => accountService.CreateUserAsync(registerData));
            
        }

        [Fact]
        public async void given_proper_user_should_return_true()
        {
            var registerData = new RegisterDto()
            {
                Email = "test@test.pl",
                Password = "test"
            };

            _userRepository.Setup(x => x.CreateAsync(It.IsAny<User>())).Returns(Task.FromResult(true));

            var accountService = new AccountService(_userRepository.Object);
            var result = await accountService.CreateUserAsync(registerData);
            Assert.True(result);
        }

    }
}
