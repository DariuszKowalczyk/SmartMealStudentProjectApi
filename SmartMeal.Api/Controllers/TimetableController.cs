using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly ITimetableService _timetableService;

        public TimetableController(ITimetableService timetableService)
        {
            _timetableService = timetableService;
        }


        [HttpGet("by")]
        public async Task<IActionResult> GetTimetablesByDay(DateTime day)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));
            var response = await _timetableService.GetTimetablesByDay(day, userId);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimetableById(long id)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));
            var response = await _timetableService.GetTimetableById(id, userId);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTimetable(TimetableBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userId = long.Parse(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));
            var response = await _timetableService.CreateTimetable(model, userId);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTimetableById(long id)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));
            var response = await _timetableService.DeleteTimetableById(id, userId);
            if (!response.IsError)
            {
                return Ok();
            }
            return BadRequest(response.Errors);
        }

    }
}