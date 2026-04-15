using Mappy.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Mappy.Infrastructure;

public static class MiddlewarePipeline
{
    public static IApplicationBuilder AddEventualConsistency(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        
        return app;
    }
}