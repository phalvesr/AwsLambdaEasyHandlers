using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AwsLambdaEasyHandlers.Extensions;

public static class LambdaHandlersExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly, ServiceLifetime handlerLifetime = ServiceLifetime.Scoped)
    {
        var handlers = assembly.GetTypes()
            .Where(x => !x.IsInterface && !x.IsAbstract && x.GetCustomAttributes(false).Any(xx => xx.GetType() == typeof(HandlerNameAttribute)))
            .Select(x => (x))
            .ToArray();

        foreach (var handler in handlers)
        {
            services.Add(new ServiceDescriptor(typeof(IBaseHandler), handler, handlerLifetime));
        }

        return services;
    }
    
    public static IServiceCollection AddContextWithHandlers(this IServiceCollection services, Assembly assembly, ServiceLifetime handlerLifetime = ServiceLifetime.Scoped)
    {
        AddHandlers(services, assembly, handlerLifetime);

        services.TryAddSingleton<IHandlerContext>(sp =>
        {
            var context = new HandlerContextDefault(sp);
            
            return context;
        });
        
        return services;
    }
}