using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Mappy.Application.Common.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel;
using SharedKernel.RabbitMQ;

namespace Mappy.Infrastructure.Services;

public class DestinationsService: IDestinationsService
{
    private readonly RabbitMQService _rabbitMq;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<DestinationsResponse>> _destinations = [];
    
    public DestinationsService(RabbitMQService rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }
    
    public async Task<DestinationsResponse> SearchAsync(string? input = null, CancellationToken cancellationToken = default)
    {
        await using var channel = await _rabbitMq.CreateChannelAsync();
        var consumer = new AsyncEventingBasicConsumer(channel);
        var replyQueueName = (await channel.QueueDeclareAsync(cancellationToken: cancellationToken)).QueueName; 
        
        consumer.ReceivedAsync += async (@object, eventArguments) =>
        {
            string? correlationId = eventArguments.BasicProperties.CorrelationId;

            if (!string.IsNullOrEmpty(correlationId))
            {
                if (_destinations.TryRemove(correlationId, out var taskCompletionSource))
                {
                    byte[] body = eventArguments.Body.ToArray();
                    string message = Encoding.UTF8.GetString(body);
                    DestinationsResponse? response = JsonSerializer.Deserialize<DestinationsResponse>(message);
                    if (response is null)
                    {
                        taskCompletionSource.SetCanceled(cancellationToken);
                    }
                    else
                    {
                        taskCompletionSource.TrySetResult(response);
                    }
                }
            }

            await channel.BasicConsumeAsync(replyQueueName, false, consumer, cancellationToken);
        };
        
        await channel.BasicConsumeAsync(replyQueueName, false, consumer, cancellationToken);
        
        string messageCorrelationId = Guid.NewGuid().ToString();
        
        BasicProperties basicProperties = new BasicProperties
        {
            CorrelationId = messageCorrelationId,
            ReplyTo = replyQueueName
        };

        TaskCompletionSource<DestinationsResponse> tcs = new TaskCompletionSource<DestinationsResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

        _destinations.TryAdd(messageCorrelationId, tcs);

        GetDestinationsRequest request = new GetDestinationsRequest(input);
        string requestMessage = JsonSerializer.Serialize(request);
        byte[] requestBody = Encoding.UTF8.GetBytes(requestMessage);

        await channel.BasicPublishAsync(string.Empty, routingKey: "Rpc_Queue", false, basicProperties, requestBody, cancellationToken);

        await using CancellationTokenRegistration ctr =
            cancellationToken.Register(() =>
            {
                _destinations.TryRemove(messageCorrelationId, out _);
                tcs.SetCanceled(cancellationToken);
            });

        return await tcs.Task;
    }
}
