using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartMeal.Data.Repository.Interfaces;
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

//        [Fact]
//        public async void should_create_new_recipe_without_photo_and_with_ingredients()
//        {
//            var recipeBindingModel = new RecipeBindingModel()
//            {
//                Name = "test",
//                Description = "Description test",
//                ImagePath = "",
//                Ingredients = null
//            };
//            var user = new User()
//            {
//                Id = 1,
//                Email = "test@test.pl"
//            };
//
//            Mock<IRepository<Photo>> _photoRepository = new Mock<IRepository<Photo>>();
//            _photoRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<bool>()))
//                .ReturnsAsync((Photo)null);
//            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
//            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
//                .ReturnsAsync(user);
//            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
//            _recipeRepository.Setup(x => x.AnyExist(It.IsAny<Expression<Func<Recipe, bool>>>()))
//                .Returns(Task.FromResult(false));
//            _recipeRepository.Setup(x => x.CreateAsync(It.IsAny<Recipe>())).ReturnsAsync(true);
//            Mock<IIgredientService> _ingredientService = new Mock<IIgredientService>();
//            _ingredientService.Setup(x => x.CreateIngredientsToRecipe(0, null)).ReturnsAsync(new Responses<IngredientDto>());
//
//            var recipeService = new RecipeService(_recipeRepository.Object, _ingredientService.Object, _userRepository.Object, _photoRepository.Object);
//
//            var response = await recipeService.CreateRecipeAsync(recipeBindingModel, user.Id);
//
//            Assert.False(response.IsError);
//            Assert.Equal(recipeBindingModel.Name, response.Data.Name);
//            Assert.Equal(recipeBindingModel.Description, response.Data.Description);
//            Assert.Equal(null, response.Data.ImagePath);
//            Assert.Equal(new List<IngredientDto>(), response.Data.Ingredients);
//        }
    }
}
