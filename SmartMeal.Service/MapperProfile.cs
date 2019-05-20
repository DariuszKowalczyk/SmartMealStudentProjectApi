using System.Collections.Generic;
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
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(i => i.Image.Filename));
            CreateMap<ProductBindingModel, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

            // Recipe
            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());
            CreateMap<RecipeBindingModel, Recipe>()
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

            // Ingredient
            CreateMap<IngredientBindingModel, Ingredient>()
                .ForMember(dest => dest.Recipe, opt => opt.Ignore());

            CreateMap<Ingredient, IngredientDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom((x => Mapper.Map<ProductDto>(x.Product))));

            // Timetable
            CreateMap<TimetableBindingModel, Timetable>()
                .ForMember(dest => dest.Recipe, opt => opt.Ignore());
            CreateMap<Timetable, TimetableDto>();

            // Photo
            CreateMap<Photo, PhotoDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(i => i.Filename));

            // User
            CreateMap<FacebookUserData, User>()
                .ForMember(i => i.Password, opt => opt.Ignore())
                .ForMember(i => i.Id, opt => opt.Ignore());

        }

    }
}
