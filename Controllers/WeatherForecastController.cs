using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreServiceSignalRSample.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoreServiceSignalRSample.Controllers
{
    [ApiController]
    //[Route("[controller]")] // this will make all route to controller, will fail if more than one method
    [Route("api/[controller]/[action]")]    // set the route so call can be aip/controller/action
    public class WeatherForecastController : ControllerBase
    {
        IBGService bgService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBGService bgService)
        {
            _logger = logger;
            this.bgService = bgService;
        }

        [HttpGet]
        public IEnumerable<string> GetGames()
        {
            return this.bgService.getGameNames();
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
        public bool AddGame(int index)
        {
            Task<bool> task = this.bgService.addGame("Game " + Convert.ToString(index));
            Console.WriteLine("After AddGame: " + JsonConvert.SerializeObject(this.bgService.getGameNames(), Formatting.Indented));
            return task.Result;
        }
    }
}
