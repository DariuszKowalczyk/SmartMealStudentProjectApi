using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
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
        private readonly IConverter _converter;

        public TimetableController(ITimetableService timetableService, IConverter converter)
        {
            _timetableService = timetableService;
            _converter = converter;
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

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdfTimetableByDay(DateTime day)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));
            var response = await _timetableService.GetTimetablesByDay(day, userId);
            var timetables = response.Data;

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = $"Harmonogram {day.Date}"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = GenerateTimetablePdfByDay(day, timetables),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);

            return File(file, "application/pdf");
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

        private string GenerateTimetablePdfByDay(DateTime day, List<TimetableDto> timetables)
        {
            var sb = new StringBuilder();
            
            if (timetables.Count == 0)
            {
                sb.AppendFormat("<h2>Brak rozpisanych posiłków na ten dzień :(</h2>");
            }
            else
            {
                sb.AppendFormat(@"<html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Twój harmonogram żywnienia na {0} {1}</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Rodzaj posiłku</th>
                                        <th>Danie</th>
                                    </tr>", day.DayOfWeek, day.Date);
                foreach (var timetable in timetables)
                {
                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                  </tr>", timetable.MealTime, timetable.Recipe.Name);
                }
            }

            sb.Append(@"
                            </table>
                        </body>
                    </html>");

            return sb.ToString();
        }
    }
}