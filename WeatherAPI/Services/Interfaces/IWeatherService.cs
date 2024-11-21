namespace WeatherAPI.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<string> GetWeather(string city, string country);
    }
}
