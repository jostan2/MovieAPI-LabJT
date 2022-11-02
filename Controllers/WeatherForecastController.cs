using Microsoft.AspNetCore.Mvc;

namespace Pets_API_Practice.Controllers
{
    [ApiController]
    [Route("[controller]")] //[controller] is hidden, url will be "WeatherForcast"
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //HTTpGet tag does 2 things, tells .NET the type of http call it
        //is (get), and tells the route to this endpoint
        //Each endpoint gets its own method.

        //c# in an API takes what ever data type your action returns and converts it into JSON automatically. 

        [HttpGet(Name = "GetWeatherForecast")] 
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast //generates 5 random forcasts, then convert into an array
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}