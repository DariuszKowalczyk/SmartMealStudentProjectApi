using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(ProductDto product);
        Task<bool> DeleteProductAsync(long id);
        Task<Product> GetProductById(long id);

    }
}
