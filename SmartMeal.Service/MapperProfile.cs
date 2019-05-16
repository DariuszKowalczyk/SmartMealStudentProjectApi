﻿using System.Collections.Generic;
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

            // Recipe
            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());
            CreateMap<RecipeBindingModel, Recipe>()
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore());

            // Ingredient
            CreateMap<IngredientBindingModel, Ingredient>()
                .ForMember(dest => dest.Recipe, opt => opt.Ignore())
                .ForMember(dest => dest.Recipe, opt => opt.Ignore());
            CreateMap<Ingredient, IngredientDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(i => i.Product.Name));
        }

    }
}
