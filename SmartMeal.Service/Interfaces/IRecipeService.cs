using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IRecipeService
    {
        Task<List<Recipe>> GetRecipies();

        Task<bool> CreateProductAsync(RecipeDto recipe);

        Task<bool> DeleteRecipeAsync(long id);

        Task<Recipe> GetRecipeById(long id);

    }
}
