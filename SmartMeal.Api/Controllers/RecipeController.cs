using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
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

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _recipeService.CreateProductAsync(model);
            if (result)
            {
                return Ok();
            }
            else
            {
                return Conflict(ModelState);
            }

        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteRecipe(long id)
        {
            var result = await _recipeService.DeleteRecipeAsync(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("get{id}")]
        public async Task<IActionResult> GetSingleRecipe(long id)
        {
            var result = await _recipeService.GetRecipeById(id);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetRecipes()
        {
            var result = await _recipeService.GetRecipies();

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("photo")]
        public async Task<string> UploadRecipeImage([FromForm(Name = "file")] IFormFile file)
        {
            string newFilename;

            if (file.Length > 0)
            {
                string imagePath;
                do
                {
                    newFilename = Guid.NewGuid().ToString().Substring(0, 15) + Path.GetExtension(file.FileName);
                    imagePath = _imagePath + newFilename;
                } while (System.IO.File.Exists(imagePath));


                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    stream.Position = 0;

                    await file.CopyToAsync(stream);
                }

                return newFilename;
            }

            return "Błąd!";
        }
    }
}