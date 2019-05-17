using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IRecipeService
    {
        Task<Responses<RecipeDto>> GetRecipes();

        Task<Response<RecipeDto>> CreateRecipeAsync(RecipeBindingModel model);

        Task<Response<RecipeDto>> GetRecipeById(long id);

        Task<Response<RecipeDto>> UpdateRecipeAsync(RecipeBindingModel model, long id);

        Task<Response<DtoBaseModel>> DeleteRecipeAsync(long id);

        

    }
}
