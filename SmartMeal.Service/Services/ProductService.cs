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
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Photo> _photoRepository;

        public ProductService(IRepository<Product> productRepository, IRepository<User> userRepository, IRepository<Photo> photoRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _photoRepository = photoRepository;
        }

        public async Task<Responses<ProductDto>> GetProducts()
        {
            var response = new Responses<ProductDto>();
            var products = await _productRepository.GetAllAsync(includes: param => param.Image);

            List<ProductDto> productsDto = new List<ProductDto>();
            foreach (var product in products)
            {
                productsDto.Add(Mapper.Map<ProductDto>(product));
            }
            response.Data = productsDto;

            return response;
        }

        public async Task<Response<ProductDto>> CreateProductAsync(ProductBindingModel model, long userId)
        {
            var response = new Response<ProductDto>();

            var productExist = await _productRepository.AnyExist(x => x.Name == model.Name);
            if (productExist)
            {
                response.AddError(Error.ProductExist);
                return response;
            }
            var user = await _userRepository.GetByAsync(x => x.Id == userId, withTracking: true);
            var photo = await _photoRepository.GetByAsync(x => x.Filename == model.ImagePath, withTracking:true);
            var product = Mapper.Map<Product>(model);
            product.CreatedBy = user;
            product.Image = photo;

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
            var product = await _productRepository.GetByAsync(x => x.Id == id, includes: param => param.Image);
            if (product == null)
            {
                response.AddError(Error.ProductDoesntExist);
                return response;
            }

            var productDto = Mapper.Map<ProductDto>(product);
            
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

            if (product.Image != null)
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
