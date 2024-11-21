using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeatherAPI.Configs;

public class LimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ApiKeyConfig _apiKeyConfig;
    private const int MaxRequestsPerKey = 5;
    private const int TimeWindowInHours = 1;
    private readonly object _lockObject = new object();

    public LimitMiddleware(RequestDelegate next, IMemoryCache cache, IOptions<ApiKeyConfig> apiKeyConfig)
    {
        _next = next;
        _cache = cache;
        _apiKeyConfig = apiKeyConfig.Value;
    }

    private string GetAvailableApiKey()
    {
        lock (_lockObject)
        {
            foreach (var apiKey in _apiKeyConfig.ApiKeys)
            {
                var cacheKey = $"RateLimit_{apiKey}";
                var requestCount = _cache.GetOrCreate(cacheKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(TimeWindowInHours);
                    return 0;
                });

                if (requestCount < MaxRequestsPerKey)
                {
                    _cache.Set(cacheKey, requestCount + 1, TimeSpan.FromHours(TimeWindowInHours));
                    return apiKey;
                }
            }
            return null;
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/Weather"))
        {
            await _next(context);
            return;
        }

        var availableApiKey = GetAvailableApiKey();

        if (availableApiKey == null)
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsync("Rate limit exceeded. All API keys have reached their hourly limit of 5 requests.");
            return;
        }

        context.Items["SelectedApiKey"] = availableApiKey;

        await _next(context);
    }
}
