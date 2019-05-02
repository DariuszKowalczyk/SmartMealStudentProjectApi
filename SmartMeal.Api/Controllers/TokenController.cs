using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)));
            }

            IActionResult response = Unauthorized();

            var token = await _tokenService.Authenticate(login);

            if (token != null)
            {
                return Ok(token);
            }

            return response;
        }
    }
}