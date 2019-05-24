using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service;
using SmartMeal.Service.Services;
using Xunit;

namespace SmartMeal.Tests.UnitTests.Services
{
    public class TimeTableServiceTests
    {
        public TimeTableServiceTests()
        {
            AutoMapperConfig.Initialize();
        }

        [Fact]
        public async void should_return_given_timetable()
        {
          
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };
            var timetable = new List<Timetable>()
            {
                new Timetable()
                {
                    Id = 1,
                    Owner = user,
                    Recipe = recipe,
                    MealDay = new DateTime(2000, 1, 1),
                    MealTime = MealTime.Breakfast,
                },
                new Timetable()
                {
                    Id = 2,
                    Owner = user,
                    Recipe = recipe,
                    MealDay = new DateTime(2000, 1, 1),
                    MealTime = MealTime.Supper,
                }
            };
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
          
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);

            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();
            _timeTableRepository.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<Timetable, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Timetable, object>>>()))
                .ReturnsAsync(timetable);

            
            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);
            var response = await ingredientService.GetTimetablesByDay(new DateTime(2000, 1, 1), user.Id);
            Assert.False(response.IsError);
            Assert.True(response.Data.Count == 2);
            Assert.True(response.Data[0].Id == 1);
            Assert.True(response.Data[1].Id == 2);
            Assert.True(response.Data[0].Recipe.Id == recipe.Id);
            Assert.True(response.Data[1].Recipe.Id == recipe.Id);
        }

        [Fact]
        public async void should_return_empty_array()
        {
       
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
          
            var timetable = new List<Timetable>() {};
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);

            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();
            _timeTableRepository.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<Timetable, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Timetable, object>>>()))
                .ReturnsAsync(timetable);


            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);
            var response = await ingredientService.GetTimetablesByDay(new DateTime(2000, 1, 1), user.Id);
            Assert.False(response.IsError);
            Assert.True(response.Data.Count == 0);

        }
        [Fact]
        public async void given_ingredient_should_create_new_timetable()
        {
            var ingredientsBindingModel = new TimetableBindingModel()
            {
                MealDay = new DateTime(2000, 1, 1),
                MealTime = MealTime.Breakfast,
                RecipeId = 1,
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };
         
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);

            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();
            _timeTableRepository.Setup(x => x.CreateAsync(It.IsAny<Timetable>())).ReturnsAsync(true);

            
            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);
            var response = await ingredientService.CreateTimetable(ingredientsBindingModel,user.Id);

            Assert.False(response.IsError);
            Assert.True(response.IsValid);
            Assert.Equal(response.Data.Recipe.Id, recipe.Id);
            Assert.Equal(response.Data.MealDay, new DateTime(2000,1,1));
        }
        [Fact]
        public async void given_ingredient_should_return_recipe_doesnt_exist_error()
        {
            var ingredientsBindingModel = new TimetableBindingModel()
            {
                MealDay = new DateTime(2000, 1, 1),
                MealTime = MealTime.Breakfast,
                RecipeId = 1,
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
          

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Recipe)null);

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();

            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);
            var response = await ingredientService.CreateTimetable(ingredientsBindingModel, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.RecipeDoesntExist, response.Errors[0].Message);

        }
        [Fact]
        public async void given_user_should_return_user_doesnt_exist()
        {
            var ingredientsBindingModel = new TimetableBindingModel()
            {
                MealDay = new DateTime(2000, 1, 1),
                MealTime = MealTime.Breakfast,
                RecipeId = 1,
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };


            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((User) null);
            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();

            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);
            var response = await ingredientService.CreateTimetable(ingredientsBindingModel, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.UserDoesntExist, response.Errors[0].Message);

        }
        [Fact]
        public async void given_ingredient_should_return_create_error()
        {
            var ingredientsBindingModel = new TimetableBindingModel()
            {
                MealDay = new DateTime(2000, 1, 1),
                MealTime = MealTime.Breakfast,
                RecipeId = 1,
            };
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };


            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);
            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();
            _timeTableRepository.Setup(x => x.CreateAsync(It.IsAny<Timetable>())).ReturnsAsync(false);

            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);
            var response = await ingredientService.CreateTimetable(ingredientsBindingModel, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.TimeTableCreateFails, response.Errors[0].Message);

        }
        [Fact]
        public async void given_id_should_return_timetable()
        {
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };
            var timetable = new Timetable
            {
                    Id = 1,
                    Owner = user,
                    Recipe = recipe,
                    MealDay = new DateTime(2000, 1, 1),
                    MealTime = MealTime.Breakfast,
            
            };
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();

            _timeTableRepository
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Timetable, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Timetable, object>>>()))
                .ReturnsAsync(timetable);

            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);

            var response = await ingredientService.GetTimetableById(1, user.Id);

            Assert.False(response.IsError);
            Assert.Equal(response.Data.Id, timetable.Id);
            Assert.Equal(response.Data.Recipe.Id, recipe.Id);
            Assert.Equal(response.Data.MealDay, new DateTime(2000, 1, 1));
        }
        [Fact]
        public async void given_id_should_return_timetable_doesnt_exist()
        {
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
           
      
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();

            _timeTableRepository
                .Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Timetable, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Timetable, object>>>()))
                .ReturnsAsync((Timetable) null);

            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);

            var response = await ingredientService.GetTimetableById(1, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.TimetableDoesntExist, response.Errors[0].Message);
        }

        [Fact]
        public async void timetable_with_given_id_should_be_deleted()
        {
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };
            var timetable = new Timetable
            {
                Id = 1,
                Owner = user,
                Recipe = recipe,
                MealDay = new DateTime(2000, 1, 1),
                MealTime = MealTime.Breakfast,

            };

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();
            _timeTableRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Timetable, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(timetable);
            _timeTableRepository.Setup(x => x.RemoveElement(It.IsAny<Timetable>())).ReturnsAsync(true);

            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);

            var response = await ingredientService.DeleteTimetableById(timetable.Id, user.Id);

            Assert.False(response.IsError);
            Assert.Null(response.Data);
        }
        [Fact]
        public async void timetable_with_given_id_should_return_doenst_exist_error()
        {
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };
            var timetable = new Timetable
            {
                Id = 1,
                Owner = user,
                Recipe = recipe,
                MealDay = new DateTime(2000, 1, 1),
                MealTime = MealTime.Breakfast,

            };

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();
            _timeTableRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Timetable, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Timetable)null);

            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);

            var response = await ingredientService.DeleteTimetableById(timetable.Id, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.TimetableDoesntExist, response.Errors[0].Message);
        }

        [Fact]
        public async void timetable_with_given_id_should_return_create_failure()
        {
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };
            var timetable = new Timetable
            {
                Id = 1,
                Owner = user,
                Recipe = recipe,
                MealDay = new DateTime(2000, 1, 1),
                MealTime = MealTime.Breakfast,

            };

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Timetable>> _timeTableRepository = new Mock<IRepository<Timetable>>();
            _timeTableRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Timetable, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(timetable);
            _timeTableRepository.Setup(x => x.RemoveElement(It.IsAny<Timetable>())).ReturnsAsync(false);
            var ingredientService = new TimetableService(_timeTableRepository.Object, _userRepository.Object, _recipeRepository.Object);

            var response = await ingredientService.DeleteTimetableById(timetable.Id, user.Id);

            Assert.True(response.IsError);
            Assert.Equal(Error.TimeTableCreateFails, response.Errors[0].Message);
        }


    }
}


