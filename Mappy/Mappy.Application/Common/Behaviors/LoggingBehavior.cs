using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Mappy.Application.Common.Behaviors;

public class LoggingBehavior<TRequest>: IRequestPreProcessor<TRequest> where TRequest: notnull
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receiving request {@Request}", request);
        
        return Task.CompletedTask;
    }
}