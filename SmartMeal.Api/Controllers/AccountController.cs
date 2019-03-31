
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartMeal.Models;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto
                {
                    errors = new List<string> { Error.RegisterDtoIsNotValid }
                });
            }

            try
            {
                await _accountService.CreateUserAsync(registerDto);
                
            }
            catch (SmartMealException exception)
            {
                return BadRequest(new ErrorDto
                {
                    errors = exception.errors
                });
            }
            return Ok();
        }
    }
}