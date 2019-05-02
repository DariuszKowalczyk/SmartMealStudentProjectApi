

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Services;
using Xunit;

namespace SmartMeal.Tests.UnitTests
{
    public class TokenServiceTests
    {
        private Mock<IRepository<User>> _userRepository { get; set; }
        private Mock<IConfiguration> _config { get; set; }
        public TokenServiceTests()
        {
            _userRepository = new Mock<IRepository<User>>();
            _config = new Mock<IConfiguration>();
            _config.Setup(x => x["Jwt:Key"]).Returns("veryVerySecretKey123");
            _config.Setup(x => x["Jwt:Issuer"]).Returns("http://localhost:63939/");
        }


        [Fact]
        public async void given_token_and_success_authentication()
        {
            var loginDto = new LoginDto()
            {
                Email = "test@test.pl",
                Password = "SecretKey123"
            };
            var user = new User()
            {
                Email =  "test@test.pl",
                Password = "SecretKey123"
            };
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult(user));
             
            var tokenService = new TokenService(_config.Object, _userRepository.Object);

            var result = await tokenService.Authenticate(loginDto);

            Assert.IsType<AuthDto>(result);

        }

        [Fact]
        public async void given_wrong_data_should_return_false()
        {
            var loginDto = new LoginDto()
            {
                Email = "test@test.pl",
                Password = "SecretKey"
            };
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult<User>(null));

            var tokenService = new TokenService(_config.Object, _userRepository.Object);

            var result = await tokenService.Authenticate(loginDto);

            Assert.Null(result);

        }
    }
}
