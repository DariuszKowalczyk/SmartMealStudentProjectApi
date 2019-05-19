using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IAuthService
    {
        Task<Response<TokenDto>> Authenticate(AuthBindingModel model);
        Task<Response<TokenDto>> CreateToken(UserAuthBindingModel model);
    }
}
