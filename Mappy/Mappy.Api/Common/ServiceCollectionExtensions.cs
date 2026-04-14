using System.Reflection;

namespace Mappy.Api.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSlices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var slices = assembly.GetTypes()
            .Where(type => typeof(ISlice).IsAssignableFrom(type)
            && !type.IsAbstract
            && type != typeof(ISlice)
            && type.IsPublic);

        foreach (var slice in slices)
            services.AddSingleton(typeof(ISlice), slice);

        return services;
    }
}