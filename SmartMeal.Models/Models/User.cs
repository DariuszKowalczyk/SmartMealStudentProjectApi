using Microsoft.AspNetCore.Identity;

namespace SmartMeal.Models.Models
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
