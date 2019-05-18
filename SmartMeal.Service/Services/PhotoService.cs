using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SmartMeal.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly string _imagePath;
        private readonly IHostingEnvironment _environment;
        private readonly List<string> _acceptContentType = new List<string>()
        {
            "image/jpg", "image/jpeg", "image/png", "image/gif"
        };


        public PhotoService(IHostingEnvironment environment)
        {
            _environment = environment;
            _imagePath = _environment.ContentRootPath + "\\Images\\";
        }

        public async Task<Response<PhotoDto>> UploadPhotoAsync(IFormFile file)
        {
            var response = new Response<PhotoDto>();
            if (file == null || file.Length < 0)
            {
               response.AddError(Error.FileIsEmpty);
               return response;

            }
            // Valid content type
            if (!_acceptContentType.Any(x => x == file.ContentType))
            {
                response.AddError(Error.FileWrongContentType);
                return response;

            }

            var fileName = generateImageName(Path.GetExtension(file.FileName));
            using (var stream = new FileStream($"{_imagePath}{fileName}", FileMode.Create))
            {
                stream.Position = 0;

                await file.CopyToAsync(stream);
            }
            var photoDto = new PhotoDto(){ImagePath = fileName };

            response.Data = photoDto;
            return response;
        }


        private string generateImageName(string extension)
        {
            var imagePath = "";
            var newFilename = "";
            do
            {
                newFilename = Guid.NewGuid().ToString().Substring(0, 15) + extension;
                imagePath = $"{_imagePath}{newFilename}";
            } while (System.IO.File.Exists(imagePath));
            return newFilename;
        }
    }
}
