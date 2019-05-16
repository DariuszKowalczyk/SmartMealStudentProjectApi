﻿using System;
using System.Collections.Generic;
using System.Text;
using SmartMeal.Models.Models;

namespace SmartMeal.Models.ModelsDto
{
    public class RecipeDto : DtoBaseModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public List<IngredientDto> Ingredients { get; set; }

    }
}
