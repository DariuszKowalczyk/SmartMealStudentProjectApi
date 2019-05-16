using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Remotion.Linq.Clauses;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepository<Recipe> _recipeRepository;
        private readonly IIgredientService _igredientService;

        public RecipeService(IRepository<Recipe> recipeRepository, IIgredientService inIgredientService)
        {
            _recipeRepository = recipeRepository;
            _igredientService = inIgredientService;
        }


        public async Task<Responses<RecipeDto>> GetRecipes()
        {
            var response = new Responses<RecipeDto>();
            var recipes = await _recipeRepository.GetAllAsync();

            List<RecipeDto> recipesDto = new List<RecipeDto>();
            foreach (var recipe in recipes)
            {
                recipesDto.Add(Mapper.Map<RecipeDto>(recipe));
            }

            response.Data = recipesDto;

            return response;
        }

        public async Task<Response<RecipeDto>> CreateRecipeAsync(RecipeBindingModel model)
        {
            var response = new Response<RecipeDto>();

            var recipeExist = await _recipeRepository.AnyExist(x => x.Name == model.Name);
            if (recipeExist)
            {
                response.AddError(Error.RecipeExist);
                return response;
            }

            var recipe = Mapper.Map<Recipe>(model);

            bool isCreated = await _recipeRepository.CreateAsync(recipe);

            if (!isCreated)
            {
                response.AddError("błąd Podczas tworzenia przepisu!");
                return response;
            }

            // Dodawanie składników
            var result = await _igredientService.CreateIngredientsToRecipe(recipe.Id, model.Ingredients);
            if (result.IsError)
            {
                response.Errors = result.Errors;
                return response;
            }

            var recipeDto = Mapper.Map<RecipeDto>(recipe);
            recipeDto.Ingredients = result.Data;

            response.Data = recipeDto;
            return response;
            
        }

        public async Task<Response<RecipeDto>> GetRecipeById(long id)
        {
            var response = new Response<RecipeDto>();
            var recipe = await _recipeRepository.GetByAsync(x => x.Id == id);
            if (recipe == null)
            {
                response.AddError(Error.RecipeDoesntExist);
                return response;
            }

            var recipeDto = Mapper.Map<RecipeDto>(recipe);
            if (recipeDto.ImagePath != "")
            {
                recipeDto.ImagePath = $"/static/images/{recipeDto.ImagePath}";
            }

            response.Data = recipeDto;
            return response;
        }

        public async Task<Response<RecipeDto>> UpdateRecipeAsync(RecipeBindingModel model, long id)
        {
            var response = new Response<RecipeDto>();

            var recipe = await _recipeRepository.GetByAsync(x => x.Id == id);
            if (recipe == null)
            {
                response.AddError(Error.RecipeDoesntExist);
                return response;
            }

            var newRecipe = Mapper.Map<Recipe>(model);
            newRecipe.Id = id;

            bool isUpdated = await _recipeRepository.UpdateAsync(newRecipe);

            if (!isUpdated)
            {
                response.AddError("Wystąpił błąd podczas aktualizowania!");
                return response;
            }



            var recipeDto = Mapper.Map<RecipeDto>(newRecipe);
            response.Data = recipeDto;

            return response;

        }

        public async Task<Response<DtoBaseModel>> DeleteRecipeAsync(long id)
        {
            var response = new Response<DtoBaseModel>();

            var recipe = await _recipeRepository.GetByAsync(x => x.Id == id);
            if(recipe == null)
            {
               response.AddError(Error.RecipeDoesntExist);
               return response;
            }

            if (recipe.ImagePath != null)
            {

            }

            var isDeleted = await _recipeRepository.RemoveElement(recipe);

            if (!isDeleted)
            {
                response.AddError("Błąd przy usuwaniu");
            }

            return response;
        }
    }
}
