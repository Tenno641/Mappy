using System.Text;
using System.Text.Json;
using Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.RabbitMQ;

namespace DestinationSearch.API.Destinations;

public class RequestsConsumer: IHostedService
{
    private readonly RabbitMQService _rabbitmq;
    private readonly DestinationsRepository _destinationsRepository;
    
    public RequestsConsumer(RabbitMQService rabbitmq, DestinationsRepository destinationsRepository)
    {
        _rabbitmq = rabbitmq;
        _destinationsRepository = destinationsRepository;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        IChannel channel = await _rabbitmq.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "Rpc_Queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } },
            cancellationToken: cancellationToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            byte[] body = eventArgs.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);

            GetDestinationsRequest? request = JsonSerializer.Deserialize<GetDestinationsRequest>(message); // TODO: Handle Exception

            BasicProperties basicProperties = new BasicProperties()
            {
                CorrelationId = eventArgs.BasicProperties.CorrelationId,
            };

            if (request is null)
            {
                Error error = new Error(Message: "Failed serializing the request");
                DestinationsResponse emptyResponse = new DestinationsResponse([], [error]);
                string responseMessage = JsonSerializer.Serialize(emptyResponse);
                byte[] responseBody = Encoding.UTF8.GetBytes(responseMessage);
                await channel.BasicPublishAsync(string.Empty, eventArgs.BasicProperties.ReplyTo, false, basicProperties, responseBody, cancellationToken);
                await channel.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken);
                return;
            }

            List<DestinationResponse> destinations = _destinationsRepository.Search(request.Input)
                .ConvertAll(d => new DestinationResponse(
                    Name: d.Name,
                    Description: d.Description,
                    ImageUri: d.ImageUri));

            DestinationsResponse response = new DestinationsResponse(destinations);
            
            string destinationsResponse = JsonSerializer.Serialize(response);
            byte[] destinationsBody = Encoding.UTF8.GetBytes(destinationsResponse);
            
            await channel.BasicPublishAsync(string.Empty, eventArgs.BasicProperties.ReplyTo, false, basicProperties, destinationsBody, cancellationToken);
            
            await channel.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken);
        };
        
        await channel.BasicConsumeAsync("Rpc_Queue", false, consumer, cancellationToken);
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _rabbitmq.DisposeAsync();
    }
}