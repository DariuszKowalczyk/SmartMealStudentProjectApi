using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace SmartMeal.Models.Models
{
    public class Product : Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public Photo Image { get; set; }

        public User CreatedBy { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

    }
}
