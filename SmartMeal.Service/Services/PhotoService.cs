using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly string _imagePath;
        private readonly IHostingEnvironment _environment;
        private readonly IRepository<Photo> _photoRepository;
        private readonly List<string> _acceptContentType = new List<string>()
        {
            "image/jpg", "image/jpeg", "image/png", "image/gif"
        };


        public PhotoService(IHostingEnvironment environment, IRepository<Photo> photoRepository)
        {
            _environment = environment;
            _imagePath = _environment.ContentRootPath + "\\Images\\";
            _photoRepository = photoRepository;
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
            var photo = new Photo()
            {
                Filename = fileName,
                ContentType = file.ContentType,
                Size = file.Length
            };
            await _photoRepository.CreateAsync(photo);

            var photoDto = Mapper.Map<PhotoDto>(photo);

            response.Data = photoDto;
            return response;
        }

        public Task<Response<PhotoDto>> GetPhotoById(long Id)
        {
            throw new NotImplementedException();
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
