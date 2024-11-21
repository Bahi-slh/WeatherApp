using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService)
        {
            this._logger = logger;
            this._weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] string city, [FromQuery] string country)
        {
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
            {
                return BadRequest("City and country are required");
            }

            try
            {
                var result = await _weatherService.GetWeather(city, country);
                if (result == null)
                {
                    return NotFound($"No weather data found for {city}, {country}");
                }
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather for {City}, {Country}", city, country);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

    }
}
