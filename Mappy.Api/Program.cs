using Mappy.Api.Destinations;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddRabbitMqAsync();
builder.Services.AddScoped<DestinationsReceivingService>();

var app = builder.Build();

app.MapGet("api/destinations", async (string? input, [FromServices] DestinationsReceivingService service, HttpContext context) =>
{
    return Results.Ok(await service.Search(input));
});

app.Run();