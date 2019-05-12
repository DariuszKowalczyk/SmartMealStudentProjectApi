using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto model)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _productService.CreateProductAsync(model);
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
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result)
            {
                return Ok();    
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("photo")]
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

            return "Błąd!";
        }

        [HttpGet]
        [Route("get{id}")]
        public async Task<IActionResult> GetSingleProduct(long id)
        {
            var result = await _productService.GetProductById(id);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
            
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetProducts();

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }


    }
}
