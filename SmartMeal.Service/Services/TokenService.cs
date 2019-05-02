

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRepository<User> _repository;
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config, IRepository<User> repository)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<AuthDto> Authenticate(LoginDto model)
        {
            var user = await _repository.GetByAsync(x => x.Email == model.Email && x.Password == HashManager.GetHash(model.Password));


            if (user != null)
            {
                var token = BuildToken(user);
                return new AuthDto()
                {
                    Token = token
                };
            }

            return null;
        }

        private string BuildToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
