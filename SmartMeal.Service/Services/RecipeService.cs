using AutoMapper;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SmartMeal.Service.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepository<Recipe> _recipeRepository;
        private readonly IIgredientService _igredientService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Photo> _photoRepository;

        public RecipeService(IRepository<Recipe> recipeRepository, IIgredientService IgredientService, IRepository<User> userRepository, IRepository<Photo> photoRepository)
        {
            _photoRepository = photoRepository;
            _userRepository = userRepository;
            _recipeRepository = recipeRepository;
            _igredientService = IgredientService;
        }


        public async Task<Responses<RecipeDto>> GetRecipes()
        {
            var response = new Responses<RecipeDto>();
            var recipes = await _recipeRepository.GetAllAsync(includes:param => param.Image);

            List<RecipeDto> recipesDto = new List<RecipeDto>();
            foreach (var recipe in recipes)
            {
                var recipeDto = Mapper.Map<RecipeDto>(recipe);
                var result = await _igredientService.GetIngredientsFromRecipe(recipeDto.Id);
                var ingredientsDto = result.Data;
                recipeDto.Ingredients = ingredientsDto;
                recipesDto.Add(recipeDto);
            }

            response.Data = recipesDto;


            return response;
        }

        public async Task<Response<RecipeDto>> CreateRecipeAsync(RecipeBindingModel model, long userId)
        {
            var response = new Response<RecipeDto>();

            var recipeExist = await _recipeRepository.AnyExist(x => x.Name == model.Name);
            if (recipeExist)
            {
                response.AddError(Error.RecipeExist);
                return response;
            }

            var photo = await _photoRepository.GetByAsync(x => x.Filename == model.ImagePath, withTracking:true);
            var user = await _userRepository.GetByAsync(x => x.Id == userId, withTracking: true);

            var recipe = Mapper.Map<Recipe>(model);
            recipe.Image = photo;
            recipe.CreatedBy = user;

            bool isCreated = await _recipeRepository.CreateAsync(recipe);

            if (!isCreated)
            {
                response.AddError(Error.RecipeErrorWhenCreated);
                return response;
            }

            // Dodawanie składników
            var result = await _igredientService.CreateIngredientsToRecipe(recipe.Id, model.Ingredients);
            if (result.IsError)
            {
                await _recipeRepository.RemoveElement(recipe);
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
            var recipe = await _recipeRepository.GetByAsync(x => x.Id == id, includes: param => param.Image);
            if (recipe == null)
            {
                response.AddError(Error.RecipeDoesntExist);
                return response;
            }
            var result = await _igredientService.GetIngredientsFromRecipe(id);
            if (result.IsError)
            {
                response.Errors = result.Errors;
                return response;
            }

            var ingredientsDto = result.Data;
            var recipeDto = Mapper.Map<RecipeDto>(recipe);
            recipeDto.Ingredients = ingredientsDto;

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

            var isDeleted = await _recipeRepository.RemoveElement(recipe);

            if (!isDeleted)
            {
                response.AddError("Błąd przy usuwaniu");
            }

            return response;
        }
    }
}
