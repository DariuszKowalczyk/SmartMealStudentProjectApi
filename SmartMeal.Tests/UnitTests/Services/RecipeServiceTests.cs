using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service;
using SmartMeal.Service.Interfaces;
using SmartMeal.Service.Services;
using Xunit;

namespace SmartMeal.Tests.UnitTests.Services
{
    public class RecipeServiceTests
    {
        public RecipeServiceTests()
        {
            AutoMapperConfig.Initialize();
        }

        [Fact]
        public async void should_create_new_recipe_without_photo_and_without_ingredients()
        {
            var recipeBindingModel = new RecipeBindingModel()
            {
                Name = "test",
                Description = "Description test",
                ImagePath = "",
                Ingredients = null
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };

            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Photo)null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Recipe, bool>>>()))
                .Returns(Task.FromResult(false));
            _recipeRepository.Setup(x => x.CreateAsync(It.IsAny<Recipe>())).ReturnsAsync(true);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            _ingredientService.Setup(x => x.CreateIngredientsToRecipe(0, null)).ReturnsAsync(new Responses<IngredientDto>());

            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.CreateRecipeAsync(recipeBindingModel, user.Id);

            Assert.False(response.IsError);
            Assert.Equal(recipeBindingModel.Name, response.Data.Name);
            Assert.Equal(recipeBindingModel.Description, response.Data.Description);
            Assert.Equal(null, response.Data.ImagePath);
            Assert.Equal(new List<IngredientDto>(), response.Data.Ingredients);
        }

        [Fact]
        public async void should_create_new_recipe_without_photo_and_with_ingredients()
        {

            var responseIngredients = new Responses<IngredientDto>()
            {
                Data = new List<IngredientDto>()
                {
                    new IngredientDto() {Id = 1},
                    new IngredientDto() {Id = 2}
                }
            };
            var recipeBindingModel = new RecipeBindingModel()
            {
                Name = "test",
                Description = "Description test",
                ImagePath = "",
                Ingredients = null
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };

            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Photo)null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Recipe, bool>>>()))
                .Returns(Task.FromResult(false));
            _recipeRepository.Setup(x => x.CreateAsync(It.IsAny<Recipe>())).ReturnsAsync(true);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            _ingredientService.Setup(x => x.CreateIngredientsToRecipe(0, null)).ReturnsAsync(responseIngredients);

            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.CreateRecipeAsync(recipeBindingModel, user.Id);

            Assert.False(response.IsError);
            Assert.Equal(recipeBindingModel.Name, response.Data.Name);
            Assert.Equal(recipeBindingModel.Description, response.Data.Description);
            Assert.Equal(null, response.Data.ImagePath);
            Assert.Equal(responseIngredients.Data, response.Data.Ingredients);
        }

        [Fact]
        public async void should_create_new_recipe_with_photo_and_with_ingredients()
        {
            var photo = new Photo()
            {
                Id = 1,
                Filename = "test.jpg"
            };
            var responseIngredients = new Responses<IngredientDto>()
            {
                Data = new List<IngredientDto>()
                {
                    new IngredientDto() {Id = 1},
                    new IngredientDto() {Id = 2}
                }
            };
            var recipeBindingModel = new RecipeBindingModel()
            {
                Name = "test",
                Description = "Description test",
                ImagePath = "",
                Ingredients = null
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };

            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(photo);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Recipe, bool>>>()))
                .Returns(Task.FromResult(false));
            _recipeRepository.Setup(x => x.CreateAsync(It.IsAny<Recipe>())).ReturnsAsync(true);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            _ingredientService.Setup(x => x.CreateIngredientsToRecipe(0, null)).ReturnsAsync(responseIngredients);

            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.CreateRecipeAsync(recipeBindingModel, user.Id);

            Assert.False(response.IsError);
            Assert.Equal(recipeBindingModel.Name, response.Data.Name);
            Assert.Equal(recipeBindingModel.Description, response.Data.Description);
            Assert.Equal(photo.Filename, response.Data.ImagePath);
            Assert.Equal(responseIngredients.Data, response.Data.Ingredients);
        }

        [Fact]
        public async void should_create_given_error_recipe_exist()
        {
            var recipeBindingModel = new RecipeBindingModel()
            {
                Name = "test",
                Description = "Description test",
                ImagePath = "",
                Ingredients = null
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Recipe, bool>>>()))
                .Returns(Task.FromResult(true));
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.CreateRecipeAsync(recipeBindingModel, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.RecipeExist, response.Errors[0].Message);
        }

        [Fact]
        public async void should_create_given_error_when_recipe_not_created()
        {
            var recipeBindingModel = new RecipeBindingModel()
            {
                Name = "test",
                Description = "Description test",
                ImagePath = "",
                Ingredients = null
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Photo)null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Recipe, bool>>>()))
                .Returns(Task.FromResult(false));
            _recipeRepository.Setup(x => x.CreateAsync(It.IsAny<Recipe>())).ReturnsAsync(false);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.CreateRecipeAsync(recipeBindingModel, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.RecipeErrorWhenCreated, response.Errors[0].Message);
        }
        [Fact]
        public async void given_recipe_should_fail_on_ingredients_create()
        {
            var responseIngredients = new Responses<IngredientDto>()
            {
                Errors = new List<ErrorDto>()
                {
                    new ErrorDto(){Message = Error.IngredientErrorWhenCreated}
                }
            };
            var recipeBindingModel = new RecipeBindingModel()
            {
                Name = "test",
                Description = "Description test",
                ImagePath = "",
                Ingredients = null
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Photo)null);
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Recipe, bool>>>()))
                .Returns(Task.FromResult(false));
            _recipeRepository.Setup(x => x.CreateAsync(It.IsAny<Recipe>())).ReturnsAsync(true);
            _recipeRepository.Setup(x => x.RemoveElement(It.IsAny<Recipe>())).ReturnsAsync(true);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            _ingredientService.Setup(x => x.CreateIngredientsToRecipe(It.IsAny<long>(), It.IsAny<List<IngredientBindingModel>>()))
                .ReturnsAsync(responseIngredients);
            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.CreateRecipeAsync(recipeBindingModel, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.IngredientErrorWhenCreated, response.Errors[0].Message);
        }

        [Fact]
        public async void should_delete_recipe()
        {
            var recipe = new Recipe()
            {
                Id = 1
            }; 
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);
            _recipeRepository.Setup(x => x.RemoveElement(It.IsAny<Recipe>())).ReturnsAsync(true);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.DeleteRecipeAsync(recipe.Id);

            Assert.False(response.IsError);
        }

        [Fact]
        public async void given_fail_doesnt_exist_when_delete_recipe()
        {
            var recipe = new Recipe()
            {
                Id = 1
            };
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Recipe)null);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.DeleteRecipeAsync(recipe.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.RecipeDoesntExist, response.Errors[0].Message);
        }
        [Fact]
        public async void given_fail_when_delete_recipe()
        {
            var recipe = new Recipe()
            {
                Id = 1
            };
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);
            _recipeRepository.Setup(x => x.RemoveElement(It.IsAny<Recipe>())).ReturnsAsync(false);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.DeleteRecipeAsync(recipe.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.RecipeErrorWhenDeleted, response.Errors[0].Message);
        }

        [Fact]
        public async void should_given_recipe_by_id_with_image_and_ingredients()
        {
            
            var product = new Product()
            {
                Id = 1
            };
            var result = new Responses<IngredientDto>()
            {
                Data = new List<IngredientDto>()
                {
                    new IngredientDto()
                    {
                        Id = 1,
                        Metric = Metrics.Kilogram,
                        Amount = 1,
                        Product = new ProductDto()
                        {
                            Id = 1,
                            Description = "Teest",
                            Name = "test",
                            ImagePath = null
                        },
                    },
                    new IngredientDto()
                    {
                        Id = 2,
                        Metric = Metrics.Gram,
                        Amount = 2,
                        Product = new ProductDto()
                        {
                            Id = 1,
                            Description = "Test",
                            Name = "test",
                            ImagePath = null
                        },
                    },
                }
            };
                
            var photo = new Photo()
            {
                Id = 1,
                Filename = "test.jpg",
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "Test",
                Description = "Dexcription Test",
                Image = photo,
                Ingredients = null,

            };
            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Recipe,object>>>()))
                .ReturnsAsync(recipe);
            _recipeRepository.Setup(x => x.RemoveElement(It.IsAny<Recipe>())).ReturnsAsync(false);
            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
            _ingredientService.Setup(x => x.GetIngredientsFromRecipe(It.IsAny<long>())).ReturnsAsync(result);
            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);

            var response = await recipeService.GetRecipeById(recipe.Id);

            Assert.False(response.IsError);
            Assert.Equal(recipe.Id ,response.Data.Id);
            Assert.Equal(recipe.Name, response.Data.Name);
            Assert.Equal(recipe.Image.Filename, response.Data.ImagePath);
            Assert.Equal(2, response.Data.Ingredients.Count);
        }
    }

}
