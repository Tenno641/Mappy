using RabbitMQ.Client;

namespace SharedKernel.RabbitMQ;

public sealed class RabbitMQService: IDisposable, IAsyncDisposable
{
    private IConnection _connection;

    public async Task<RabbitMQService> Init()
    {
        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        RabbitMQService rabbitMqService = new RabbitMQService { _connection = await factory.CreateConnectionAsync() };
        return rabbitMqService;
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        var channel = await _connection.CreateChannelAsync();
        
        await channel.QueueDeclareAsync(
            queue: "Destinations",
            durable: true,
            autoDelete: false,
            exclusive: false,
            arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

        return channel;
    }
    
    public void Dispose()
    {
        _connection.Dispose();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}