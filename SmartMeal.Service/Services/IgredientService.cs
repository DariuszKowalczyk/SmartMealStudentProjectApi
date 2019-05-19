using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class IgredientService : IIgredientService
    {
        private readonly IRepository<Ingredient> _igredientRepository;
        private readonly IRepository<Recipe> _recipeRepository;
        private readonly IRepository<Product> _productRepository;

        public IgredientService(IRepository<Ingredient> igredientRepository, IRepository<Recipe> recipeRepository, IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
            _igredientRepository = igredientRepository;
            _recipeRepository = recipeRepository;
        }

        public async Task<Responses<IngredientDto>> CreateIngredientsToRecipe(long recipeId, List<IngredientBindingModel> ingredientBindingModels)
        {
            var response = new Responses<IngredientDto>();
            var recipe = await _recipeRepository.GetByAsync(x => x.Id == recipeId);

            if (recipe == null)
            {
                response.AddError(Error.RecipeDoesntExist);
                return response;
            }

            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (var ingredientBind in ingredientBindingModels)
            {
                var product = await _productRepository.GetByAsync(x => x.Id == ingredientBind.ProductId, withTracking:true);
                if (product == null)
                {
                    response.AddError(Error.ProductDoesntExist);
                    return response;
                }

                var ingredient = Mapper.Map<Ingredient>(ingredientBind);
                ingredient.Recipe = recipe;
                ingredient.Product = product;
                ingredients.Add(ingredient);
            }

            var areCreated = await _igredientRepository.CreateRangeAsync(ingredients);

            if (!areCreated)
            {
               response.AddError("Błąd podczas tworzenia składników");
               return response;
            }
            List<IngredientDto> ingredientDtos = new List<IngredientDto>();
            foreach (var ingredient in ingredients)
            {
                ingredientDtos.Add(Mapper.Map<IngredientDto>(ingredient));
            }

            response.Data = ingredientDtos;
            return response;


        }

        public async Task<Responses<IngredientDto>> GetIngredientsFromRecipe(long recipeId)
        {
            var response = new Responses<IngredientDto>();
            var ingredients = await _igredientRepository.GetAllByAsync(x => x.Recipe.Id == recipeId, includes: param => param.Product);

            var ingredientsDto = new List<IngredientDto>();
            foreach (var ingrident in ingredients)
            {
                var ingredientDto = Mapper.Map<IngredientDto>(ingrident);
                ingredientsDto.Add(ingredientDto);
            }

            response.Data = ingredientsDto;
            return response;

        }
    }
}
