using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IHostingEnvironment _environment;
        private readonly string _imagePath;

        public RecipeController(IHostingEnvironment environment, IRecipeService recipeService)
        {
            _environment = environment;
            _recipeService = recipeService;
            _imagePath = _environment.ContentRootPath + "\\Images\\";
        }

        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var response = await _recipeService.GetRecipes();
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);
        }


        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userId = long.Parse(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));

            var response = await _recipeService.CreateRecipeAsync(model, userId);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleRecipe(long id)
        {
            var response = await _recipeService.GetRecipeById(id);

            if (!response.IsError)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Data);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeBindingModel model)
        {
            var response = await _recipeService.UpdateRecipeAsync(model, id);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(long id)
        {
            var response = await _recipeService.DeleteRecipeAsync(id);
            if (!response.IsError)
            {
                return Ok();
            }
            return BadRequest(response.Errors);

        }
    }
}