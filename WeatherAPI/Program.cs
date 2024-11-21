using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WeatherAPI.Configs;
using WeatherAPI.Services;
using WeatherAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000") 
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Weather API",
        Version = "v1",
        Description = "Returns weather descriptions for given city and country"
    });
});


builder.Services.Configure<ApiKeyConfig>(options =>
{
    options.ApiKeys = builder.Configuration.GetSection("ApiKeys").Get<List<string>>();
});
builder.Services.AddControllers();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowReactApp");

app.UseHttpsRedirection();
app.UseMiddleware<LimitMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
