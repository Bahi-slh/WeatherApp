using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string[] _openWeatherMapKeys = new[]
        {
        "8b7535b42fe1c551f18028f64e8688f7",
        "9f933451cebf1fa39de168a29a4d9a79"
    };
        private int _currentKeyIndex = 0;
        private readonly object _lockObject = new object();
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string GetOpenWeatherMapKey()
        {
            lock (_lockObject)
            {
                _currentKeyIndex = (_currentKeyIndex + 1) % _openWeatherMapKeys.Length;
                return _openWeatherMapKeys[_currentKeyIndex];
            }
        }


        public async Task<string> GetWeather(string city, string country)
        {
            var openWeatherMapKey = GetOpenWeatherMapKey();
            var url = $"{BaseUrl}?q={city},{country}&appid={openWeatherMapKey}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var weatherData = JObject.Parse(content);
                var description = weatherData["weather"]?[0]?["description"]?.ToString();

                return JsonConvert.SerializeObject(new { description = description });
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            throw new HttpRequestException($"Weather API returned {response.StatusCode}");
        }
    }
}