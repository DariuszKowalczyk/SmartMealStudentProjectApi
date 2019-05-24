using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Service;
using SmartMeal.Service.Services;
using Xunit;

namespace SmartMeal.Tests.UnitTests.Services
{
    public class IngredientsServiceTests
    {
        public IngredientsServiceTests()
        {
            AutoMapperConfig.Initialize();
        }

        [Fact]
        public async void should_create_given_ingredients_with_recipe()
        {
            var ingredientsBindingModel = new List<IngredientBindingModel>
            {
                new IngredientBindingModel()
                {
                    ProductId = 1,
                    Metric = "1",
                    Amount = 1,
                }
            };
            var product = new Product()
            {
                Id = 1,
                Name = "test",
                Description = "DescTest",
                Image = null,
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

            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(user);

            Mock<IRepository<Ingredient>> _ingredientRepository = new Mock<IRepository<Ingredient>>();
            _ingredientRepository.Setup(x => x.CreateRangeAsync(It.IsAny<List<Ingredient>>()))
                .Returns(Task.FromResult(true));


            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(product);

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);

            var ingredientService = new IgredientService(_ingredientRepository.Object, _recipeRepository.Object, _productRepository.Object);
            var response = await ingredientService.CreateIngredientsToRecipe(1,ingredientsBindingModel);
            Assert.False(response.IsError);
            Assert.Equal(ingredientsBindingModel[0].ProductId,  response.Data[0].Product.Id);
            Assert.Equal(ingredientsBindingModel[0].Amount, response.Data[0].Amount);
        }

        [Fact]
        public async void given_recipe_doesnt_exist_should_return_error()
        {
            var ingredientsBindingModel = new List<IngredientBindingModel>
            {
                new IngredientBindingModel()
                {
                    ProductId = 1,
                    Metric = "1",
                    Amount = 1,
                }
            };
            var product = new Product()
            {
                Id = 1,
                Name = "test",
                Description = "DescTest",
                Image = null,
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
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Ingredient>> _ingredientRepository = new Mock<IRepository<Ingredient>>();

            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync((Recipe) null);

            var ingredientService = new IgredientService(_ingredientRepository.Object, _recipeRepository.Object, _productRepository.Object);
            var response = await ingredientService.CreateIngredientsToRecipe(1, ingredientsBindingModel);
            Assert.True(response.IsError);
            Assert.Equal(Error.RecipeDoesntExist, response.Errors[0].Message);
        }
        [Fact]
        public async void given_recipe_product_doesnt_exist_should_return_error()
        {
            var ingredientsBindingModel = new List<IngredientBindingModel>
            {
                new IngredientBindingModel()
                {
                    ProductId = 1,
                    Metric = "1",
                    Amount = 1,
                }
            };
            var product = new Product()
            {
                Id = 1,
                Name = "test",
                Description = "DescTest",
                Image = null,
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
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();
            
            Mock<IRepository<Ingredient>> _ingredientRepository = new Mock<IRepository<Ingredient>>();
           
            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>())).ReturnsAsync(
                (Product)null);
            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);

            var ingredientService = new IgredientService(_ingredientRepository.Object, _recipeRepository.Object, _productRepository.Object);
            var response = await ingredientService.CreateIngredientsToRecipe(1, ingredientsBindingModel);
            Assert.True(response.IsError);
            Assert.Equal(Error.ProductDoesntExist, response.Errors[0].Message);
        }
        [Fact]
        public async void given_recipe_failes_on_create_range_async()
        {
            var ingredientsBindingModel = new List<IngredientBindingModel>
            {
                new IngredientBindingModel()
                {
                    ProductId = 1,
                    Metric = "1",
                    Amount = 1,
                }
            };
            var product = new Product()
            {
                Id = 1,
                Name = "test",
                Description = "DescTest",
                Image = null,
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
            Mock<IRepository<User>> _userRepository = new Mock<IRepository<User>>();

            Mock<IRepository<Ingredient>> _ingredientRepository = new Mock<IRepository<Ingredient>>();
            _ingredientRepository.Setup(x => x.CreateRangeAsync(It.IsAny<List<Ingredient>>()))
                .Returns(Task.FromResult(false));

            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(product);

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);

            var ingredientService = new IgredientService(_ingredientRepository.Object, _recipeRepository.Object, _productRepository.Object);
            var response = await ingredientService.CreateIngredientsToRecipe(1, ingredientsBindingModel);
            Assert.True(response.IsError);
            Assert.Equal(Error.IngredientCreateFails, response.Errors[0].Message);
        }

        [Fact]
        public async void given_recipe_id_doesnt_exist_should_return_error()
        {
          
            var user = new User()
            {
                Id = 1,
                Email = "test@test.pl"
            };
            var product = new Product()
            {
                Id = 1,
                Name = "test",
                Description = "DescTest",
                Image = null,
            };
            var recipe = new Recipe()
            {
                Id = 1,
                Name = "testRecipe",
                Description = "recipeDescr",
                CreatedBy = user,
                Image = null,
            };
            var ingredientsTest = new List<Ingredient>();

            Mock<IRepository<Product>> _productRepository = new Mock<IRepository<Product>>();
            _productRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(product);

            Mock<IRepository<Recipe>> _recipeRepository = new Mock<IRepository<Recipe>>();
            _recipeRepository.Setup(x => x.GetByAsync(It.IsAny<Expression<Func<Recipe, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(recipe);

            Mock<IRepository<Ingredient>> _ingredientRepository = new Mock<IRepository<Ingredient>>();
            _ingredientRepository.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<Ingredient, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Ingredient, object>>>()))
                .ReturnsAsync(ingredientsTest);

            var ingredientService = new IgredientService(_ingredientRepository.Object, _recipeRepository.Object, _productRepository.Object);
            var response = await ingredientService.GetIngredientsFromRecipe(1);
            Assert.False(response.IsError);
            Assert.True(response.Data.Count == 0);
         
        }
    }
}


