using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.RabbitMQ;

public static class DependencyInjection
{
    public async static Task<IServiceCollection> AddRabbitMqAsync(this IServiceCollection services)
    {
        services.AddSingleton<RabbitMQService>(await new RabbitMQService().Init());

        return services;
    }
}