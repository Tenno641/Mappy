using System.Reflection;
using Mappy.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Mappy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
        });

        return services;
    }
}