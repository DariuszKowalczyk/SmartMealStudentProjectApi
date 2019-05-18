using SmartMeal.Data.Repository.Interfaces;
using SmartMeal.Models;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SmartMeal.Models.BindingModels;

namespace SmartMeal.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Responses<ProductDto>> GetProducts()
        {
            var response = new Responses<ProductDto>();
            var products = await _productRepository.GetAllAsync();

            List<ProductDto> productsDto = new List<ProductDto>();
            foreach (var product in products)
            {
                productsDto.Add(Mapper.Map<ProductDto>(product));
            }
            response.Data = productsDto;

            return response;
        }

        public async Task<Response<ProductDto>> CreateProductAsync(ProductBindingModel model)
        {
            var response = new Response<ProductDto>();

            var productExist = await _productRepository.AnyExist(x => x.Name == model.Name);
            if (productExist)
            {
                response.AddError(Error.ProductExist);
                return response;
            }

            var product = Mapper.Map<Product>(model);

            bool isCreated = await _productRepository.CreateAsync(product);

            if (isCreated)
            {   
                var productDto = Mapper.Map<ProductDto>(product);
                response.Data = productDto;
                return response;
            }

            return response;
        }

        public async Task<Response<ProductDto>> GetProductById(long id)
        {
            Response<ProductDto> response = new Response<ProductDto>();
            var product = await _productRepository.GetByAsync(x => x.Id == id);
            if (product == null)
            {
                response.AddError(Error.ProductDoesntExist);
                return response;
            }

            var productDto = Mapper.Map<ProductDto>(product);
            if (productDto.ImagePath != null)
            {
                productDto.ImagePath = $"/static/images/{productDto.ImagePath}";
            }

            response.Data = productDto;
            return response;
        }

        public async Task<Response<ProductDto>> UpdateProductAsync(ProductBindingModel model, long id)
        {
            var response = new Response<ProductDto>();

            var product = await _productRepository.GetByAsync(x => x.Id == id);
            if (product == null)
            {
                response.AddError(Error.ProductDoesntExist);
                return response;
            }

            var newProduct = Mapper.Map<Product>(model);
            newProduct.Id = id;

            bool isUpdated = await _productRepository.UpdateAsync(newProduct);

            if (!isUpdated)
            {
                response.AddError("Wystąpił błąd podczas aktualizowania!");
                return response;
            }

            var productDto = Mapper.Map<ProductDto>(newProduct);
            response.Data = productDto;

            return response;

        }

        public async Task<Response<DtoBaseModel>> DeleteProductAsync(long id)
        {
            Response<DtoBaseModel> response = new Response<DtoBaseModel>();

            var product = await _productRepository.GetByAsync(x => x.Id == id);
            if (product == null)
            {
                response.AddError(Error.ProductDoesntExist);
                return response;
            }

            if (product.ImagePath != null)
            {

            }

            var is_deleted = await _productRepository.RemoveElement(product);

            if (!is_deleted)
            {
                response.AddError("błąd usuwania");
            }

            return response;
        }
    }
}
