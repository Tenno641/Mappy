using Mappy.Domain.Common;
using Mappy.Infrastructure.Persistence;
using MediatR;
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
        
        await _next(httpContext);
        
        if (httpContext.Items.TryGetValue(MappyDbContext.DomainEventsKey, out var domainEvents)
            && domainEvents is Queue<IDomainEvent> existingDomainEvents)
        {
            while (existingDomainEvents.TryDequeue(out IDomainEvent? domainEvent))
                await publisher.Publish(domainEvent);
        }
        await transaction.CommitAsync();
    }
}