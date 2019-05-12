using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepository<Recipe> _recipeRepository;

        public RecipeService(IRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<bool> CreateProductAsync(RecipeDto recipe)
        {
            var recipeExist = await _recipeRepository.AnyExist(x => x.Name == recipe.Name);
            if (recipeExist)
            {
                throw new SmartMealException(Error.RecipeExist);
            }
            var newRecipe = new Recipe()
            {
                Name = recipe.Name,
                Description = recipe.Description
            };

            var is_created = await _recipeRepository.CreateAsync(newRecipe);

            return is_created;
        }

        public async Task<bool> DeleteRecipeAsync(long id)
        {
            var product = await _recipeRepository.GetByAsync(x => x.Id == id);
            if (product == null)
            {
                throw new SmartMealException(Error.ProductDoesntExist);
            }

            var is_deleted = await _recipeRepository.RemoveElement(product);
            return is_deleted;
        }

        public async Task<Recipe> GetRecipeById(long id)
        {
            var recipe = await _recipeRepository.GetByAsync(x => x.Id == id);
            if (recipe != null)
            {
                return recipe;
            }
            return null;
        }


        public async Task<List<Recipe>> GetRecipies()
        {
            var recipes = await _recipeRepository.GetAllAsync();
            if (recipes != null)
            {
                return recipes;
            }

            return null;

        }



    }
}
