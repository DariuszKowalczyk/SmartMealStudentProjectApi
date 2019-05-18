using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SmartMeal.Models.BindingModels;
using SmartMeal.Models.ModelsDto;
using SmartMeal.Service.Interfaces;

namespace SmartMeal.Api.Controllers
{
    [Route("api/[controller]")]
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
            var response = await _timetableService.GetTimetablesByDay(day);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimetableById(long id)
        {
            var response = await _timetableService.GetTimetableById(id);
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
            var response = await _timetableService.CreateTimetable(model);
            if (response.IsError)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Data);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTimetableById(long id)
        {
            var response = await _timetableService.DeleteTimetableById(id);
            if (!response.IsError)
            {
                return Ok();
            }
            return BadRequest(response.Errors);
        }

    }
}