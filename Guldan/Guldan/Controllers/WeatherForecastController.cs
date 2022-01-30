using Guldan.Common;
using Guldan.Data.Dto;
using Guldan.IService.Sys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldan.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISysUserService _userService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISysUserService userService
            )
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IResponse AddUser([FromBody] SysUserDto userDto)
        {

            var data = _userService.AddUser(userDto);

            _logger.LogDebug(JsonSerializer.Serialize(data));

            return ResponseInfo.Success(data);

        }
    }
}
