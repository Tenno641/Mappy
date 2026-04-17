using Mappy.Api.Common;
using Mappy.Application;
using Mappy.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SharedKernel.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddRabbitMqAsync();

builder.Services
    .AddInfrastructure()
    .AddApplication()
    .RegisterSlices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapSlicesEndpoints();
app.AddEventualConsistency();

app.Run();