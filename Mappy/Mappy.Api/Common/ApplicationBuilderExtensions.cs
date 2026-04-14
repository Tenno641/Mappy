namespace Mappy.Api.Common;

public static class ApplicationBuilderExtensions
{
    public static IEndpointRouteBuilder MapSlicesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var slices = endpointRouteBuilder.ServiceProvider.GetServices<ISlice>();
        
        foreach (var slice in slices)
            slice.AddEndPoint(endpointRouteBuilder);

        return endpointRouteBuilder;
    }
}