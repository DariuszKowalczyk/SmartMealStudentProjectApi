using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> CreateProductAsync(ProductDto product)
        {
            var productExist = await _productRepository.AnyExist(x => x.Name == product.Name);
            if (productExist)
            {
                throw new SmartMealException(Error.ProductExist);
            }
            var newProduct = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                ImagePath = product.ImagePath,
            };

            var is_created = await _productRepository.CreateAsync(newProduct);
            return is_created;
        }

        public async Task<bool> DeleteProductAsync(long id)
        {
            var product = await _productRepository.GetByAsync(x => x.Id == id);
            if (product == null)
            {
                throw new SmartMealException(Error.ProductDoesntExist);
            }
            var is_deleted = await _productRepository.RemoveElement(product);
            return is_deleted;
        }

        public async Task<Product> GetProductById(long id)
        {
            var product = await _productRepository.GetByAsync(x => x.Id == id);
            product.ImagePath = $"/static/images/{product.ImagePath}";
            if (product != null)
            {
                return product;
            }
            return null;
        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            if(products != null)
            {
                return products;
            }
            return null;
        }


    }
}
