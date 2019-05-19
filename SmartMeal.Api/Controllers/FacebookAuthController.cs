using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartMeal.Models.BindingModels;
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
        public async Task<IActionResult> Facebook([FromBody] FacebookAuthBindingModel model)
        {
            var response = await _facebookService.Authenticate(model);

            if (response.IsError)
            {
                return Unauthorized(response.Errors);
            }

            return Ok(response.Data);

        }
    }
}