using DestinationSearch.API.Destinations;
using SharedKernel.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddRabbitMqAsync();
builder.Services.AddSingleton<DestinationsRepository>();
builder.Services.AddHostedService<RequestsConsumer>();

var app = builder.Build();

app.Run();