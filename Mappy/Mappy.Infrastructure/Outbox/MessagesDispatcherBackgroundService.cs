using System.Text.Json;
using Mappy.Domain.Common;
using Mappy.Domain.Itineraries.Events;
using Mappy.Domain.Stops;
using Mappy.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mappy.Infrastructure.Outbox;

public class MessagesDispatcherBackgroundService: BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public MessagesDispatcherBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Running Shit");
            using var scope = _serviceScopeFactory.CreateScope();

            MappyDbContext dbContext = scope.ServiceProvider.GetRequiredService<MappyDbContext>();
            IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

            var outboxMessages = await dbContext.OutboxMessages
                .Where(message => !message.IsProcessed)
                .OrderBy(message => message.Id)
                .Take(50)
                .ToListAsync(stoppingToken);

            if (outboxMessages.Count > 0)
            {
                try
                {
                    foreach (var outBoxMessage in outboxMessages)
                    {
                        if (dbContext.OutboxMessages.Count(message => message.Id == outBoxMessage.Id) > 1)
                            continue;

                        Type type = Type.GetType(outBoxMessage.Type)!;

                        var domainEvent = JsonSerializer.Deserialize(outBoxMessage.Body, type);

                        Console.WriteLine($"Domain Event Before Casting {domainEvent}");
                        
                        Console.WriteLine($"Domain Event After Casting {(IDomainEvent?) domainEvent}");

                        outBoxMessage.IsProcessed = true;

                        await publisher.Publish((IDomainEvent?) domainEvent, stoppingToken);
                    }
                }
                catch (DbUpdateConcurrencyException concurrencyException)
                {

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}