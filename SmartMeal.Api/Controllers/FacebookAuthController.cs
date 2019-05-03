using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookAuthController : ControllerBase
    {

        private readonly IFacebookService _facebookService;
        private readonly IAuthService _authService;

        public FacebookAuthController(IFacebookService facebookService, IAuthService authService)
        {
            _facebookService = facebookService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Facebook([FromBody] FacebookAuthDto model)
        {
            var facebookData = await _facebookService.Authentication(model);
            if (facebookData == null)
            {
                return BadRequest();
            }

            var user = await _facebookService.GetUser(facebookData);
            if (user == null)
            {
                user = await _facebookService.Register(facebookData);
            }

            if (user != null)
            {
                var token = _authService.Authenticate(user);
                return Ok(token);
            }

            return BadRequest();
        }
    }
}