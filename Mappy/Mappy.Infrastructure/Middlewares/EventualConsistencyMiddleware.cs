using Mappy.Domain.Common;
using Mappy.Infrastructure.Persistence;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Mappy.Infrastructure.Middlewares;

public class EventualConsistencyMiddleware
{
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext, 
        MappyDbContext dbContext, 
        ILogger<EventualConsistencyMiddleware> logger,
        IPublisher publisher)
    {
        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
        httpContext.Response.OnCompleted(async () =>
        {
            try
            {
                if (httpContext.Items.TryGetValue(MappyDbContext.DomainEventsKey, out var domainEvents)
                    && domainEvents is Queue<IDomainEvent> existingDomainEvents)
                {
                    foreach (IDomainEvent @event in existingDomainEvents)
                        await publisher.Publish(@event);
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Message: {Message}", ex.Message);
            }
            finally
            {
                await transaction.RollbackAsync();
            }
        });
        
        await _next(httpContext);
    }
}