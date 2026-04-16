using Mappy.Application.Common.Interfaces;
using Mappy.Infrastructure.Persistence;
using Mappy.Infrastructure.Persistence.Repositories;
using Mappy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mappy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<IDestinationsService, DestinationsService>();
        services.AddScoped<IStopsRepository, StopsRepository>();
        services.AddScoped<IItinerariesRepository, ItinerariesRepository>();
        
        services.AddDbContext<MappyDbContext>(options =>
        {
            // options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            options.UseNpgsql("Server=localhost; Database=Mappy; Password=password; Username=postgres; Port=5432;");
        });

        // services.AddHostedService<MessagesDispatcherBackgroundService>();

        return services;
    }
}