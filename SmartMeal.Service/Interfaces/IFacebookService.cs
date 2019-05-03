using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IFacebookService
    {
        Task<FacebookUserData> Authentication(FacebookAuthDto model);
        Task<User> GetUser(FacebookUserData model);
        Task<User> Register(FacebookUserData data);
    }
}
