using Mappy.Api.Destinations;
using SharedKernel.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddRabbitMqAsync();
builder.Services.AddScoped<DestinationsReceivingService>();

var app = builder.Build();

app.MapGet("api/destinations", async (string? input, DestinationsReceivingService service) =>
{
    return Results.Ok(await service.Search(input));
});

app.Run();