using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IIgredientService
    {
        Task<Responses<IngredientDto>> CreateIngredientsToRecipe(long recipeId, List<IngredientBindingModel> ingredientBindingModels);

    }
}
