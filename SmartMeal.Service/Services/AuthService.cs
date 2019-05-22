

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartMeal.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config, IRepository<User> repository)
        {
            _userRepository = repository;
            _config = config;
        }

        public async Task<Response<TokenDto>> Authenticate(AuthBindingModel model)
        {
            var response = new Response<TokenDto>();

            var user = await _userRepository.GetByAsync(x => x.Email == model.Email && x.Password == model.Password);

            if (user == null)
            {
                response.AddError(Error.AuthenticationError);
                return response;
            }

            var token = BuildToken(user);
            var tokenDto = new TokenDto()
            {
                Token = token
            };
            response.Data = tokenDto;
            return response;
        }

        public async Task<Response<TokenDto>> CreateToken(UserAuthBindingModel model)
        {
            var response = new Response<TokenDto>();
            var user = await _userRepository.GetByAsync(x => x.Id == model.Id && x.Email == model.Email);

            if (user == null)
            {
                response.AddError(Error.AuthenticationError);
                return response;
            }

            var token = BuildToken(user);

            var tokenDto = new TokenDto()
            {
                Token = token
            };

            response.Data = tokenDto;
            return response;
        }

        private string BuildToken(User user)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var _options = new IdentityOptions();

            var claims = new[]
            {
//                new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(_options.ClaimsIdentity.UserNameClaimType, user.Id.ToString()), 
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
