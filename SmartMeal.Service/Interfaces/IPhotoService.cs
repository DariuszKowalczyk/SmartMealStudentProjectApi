using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IPhotoService
    {
        Task<Response<PhotoDto>> UploadPhotoAsync(IFormFile file);

        Task<Response<PhotoDto>> GetPhotoById(long Id);
    }
}
