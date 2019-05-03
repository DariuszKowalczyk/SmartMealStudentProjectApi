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


    }
}
