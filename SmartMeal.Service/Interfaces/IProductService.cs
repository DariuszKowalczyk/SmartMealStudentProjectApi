using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IProductService
    {
        Task<Responses<ProductDto>> GetProducts();
        Task<Response<ProductDto>> CreateProductAsync(ProductBindingModel model);
        Task<Response<ProductDto>> GetProductById(long id);
        Task<Response<ProductDto>> UpdateProductAsync(ProductBindingModel model, long id);
        Task<Response<DtoBaseModel>> DeleteProductAsync(long id);





    }
}
