using System.Security.Cryptography;
using AspireApp.ApiService;
using AspireApp.ServiceDefaults;

using Microsoft.Extensions.Caching.Distributed;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.AddRedisDistributedCache("cache");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwaggerUI(op =>
    {
        op.SwaggerEndpoint("/openapi/v1.json", "OpenApi v1");
    });
}
app.MapGet("/weatherforecast", async (IDistributedCache cache) =>
{
    await cache.SetStringAsync(DateTimeOffset.UtcNow.ToString(), DateTimeOffset.UtcNow.ToString());

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapDefaultEndpoints();
app.UseStaticFiles();

app.Run();

namespace AspireApp.ApiService
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
