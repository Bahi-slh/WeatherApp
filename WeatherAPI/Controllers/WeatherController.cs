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
            _logger.LogInformation("Fetching data for {City}, {Country}", city, country);

            try
            {
                var result = await _weatherService.GetWeather(city, country);
                if (result == null)
                {
                    _logger.LogWarning("No data found for {City}, {Country}", city, country);
                    return NotFound("No data found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching data for {City}, {Country}", city, country);
                return StatusCode(500, "Server error");
            }
        }

    }
}
