using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace SmartMeal.Models.Models
{
    public class User : Entity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public long FacebookId { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}
