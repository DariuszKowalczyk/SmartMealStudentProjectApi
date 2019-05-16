using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;


namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IHostingEnvironment _environment;
        private readonly string _imagePath;

        public ProductController(IHostingEnvironment environment, IProductService productService)
        {
            _environment = environment;
            _productService = productService;
            _imagePath = _environment.ContentRootPath + "\\Images\\";
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _productService.GetProducts();
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductBindingModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _productService.CreateProductAsync(model);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleProduct(long id)
        {
            var response = await _productService.GetProductById(id);

            if (!response.IsError)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Errors);

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductBindingModel model)
        {
            var response = await _productService.UpdateProductAsync(model, id);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (!response.IsError)
            {
                return Ok();
            }
            return BadRequest(response.Errors);
        }

        [HttpPost("photo")]
        public async Task<string> UploadProductImage([FromForm(Name = "file")] IFormFile file)
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

            return null;
        }

       

        

    }
}
