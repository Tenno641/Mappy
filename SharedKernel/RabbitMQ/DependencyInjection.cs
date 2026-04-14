using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.RabbitMQ;

public static class DependencyInjection
{
    public async static Task AddRabbitMqAsync(this IServiceCollection services)
    {
        services.AddSingleton(await new RabbitMQService().Init());
    }
}