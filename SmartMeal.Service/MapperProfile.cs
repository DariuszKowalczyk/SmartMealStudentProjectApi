using AutoMapper;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.Models;
using SmartMeal.Models.ModelsDto;

namespace SmartMeal.Service
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMappings();
        }

        private void CreateMappings()
        {
            
            // Product
            CreateMap<Product, ProductDto>();
            CreateMap<ProductBindingModel, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());



        }

    }
}
