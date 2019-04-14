using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<User> _userRepository;
        public AccountService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CreateUserAsync(RegisterDto user)
        {
            var userExist = await _userRepository.AnyExist(x => x.Email == user.Email);

            if (userExist)
            {
                throw new SmartMealException(Error.UserExist);
            }
            var newUser = new User()
            {
                Email = user.Email,
                Password = HashManager.GetHash(user.Password)
            };

            var is_created = await _userRepository.CreateAsync(newUser);

            return is_created;
        }
    }
}
