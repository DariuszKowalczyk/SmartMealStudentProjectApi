
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Register([FromBody]RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)));
            }

            var result = await _accountService.CreateUserAsync(model);
            if (result)
            {
                return Ok();
            }
            else
            {
                return Conflict(ModelState);
            }
            //catch (SmartMealException exception)
            //{
            //    return BadRequest(new ErrorDto
            //    {
            //        errors = exception.errors
            //    });
            //}
        }
    }
}