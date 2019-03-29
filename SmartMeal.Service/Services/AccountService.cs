using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Data.Repository.Interfaces;
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

        public Task<bool> CreateUserAsync(RegisterDto user)
        {
            throw new NotImplementedException();
        }
    }
}
