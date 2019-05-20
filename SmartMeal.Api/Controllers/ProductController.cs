﻿using System;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly string _imagePath;

        public ProductController(IProductService productService)
        {
            _productService = productService;
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
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] ProductBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = long.Parse(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType)); ;
            var response = await _productService.CreateProductAsync(model, userId);
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
    }
}
