using Mappy.Api.Common;
using Mappy.Application;
using Mappy.Infrastructure;
using SharedKernel.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddRabbitMqAsync();
builder.Services
    .AddInfrastructure()
    .AddApplication()
    .RegisterSlices();

var app = builder.Build();

app.MapSlicesEndpoints();
app.AddEventualConsistency();

app.Run();