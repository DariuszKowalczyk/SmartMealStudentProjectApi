using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        public ProductController(IProductService productService)
        {
            _productService = productService;
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
            var filePath = Directory.GetCurrentDirectory() + "\\ProductImages\\" + file.FileName;
            if (file.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        stream.Position = 0;
                        await file.CopyToAsync(stream);
                    }
                }
            return $"{file.FileName}";
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
