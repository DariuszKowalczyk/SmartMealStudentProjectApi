using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {            
            var response = await _photoService.UploadPhotoAsync(file);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }
    }
}