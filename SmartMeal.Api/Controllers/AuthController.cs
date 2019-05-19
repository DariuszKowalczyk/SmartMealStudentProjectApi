using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using SmartMeal.Models.BindingModels;

namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)));
            }

            var response = await _authService.Authenticate(model);

            if (response.IsError)
            {
                return Unauthorized(response.Errors);
            }
            
            return Ok(response.Data);
        }
    }
}