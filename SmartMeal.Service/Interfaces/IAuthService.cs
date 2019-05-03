using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IAuthService
    {
        Task<User> GetUserAsync(LoginDto login);
        AuthDto Authenticate(User user);
    }
}
