using Microsoft.Extensions.DependencyInjection;

namespace AwsLambdaEasyHandlers;

internal class HandlerContextDefault : IHandlerContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, IBaseHandler> _handlers = new();

    public HandlerContextDefault(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IHandler<T> GetHandler<T>()
    {
        var handlers = _serviceProvider.GetRequiredService<IEnumerable<IBaseHandler>>();

        foreach (var handler in handlers)
        {
            if (handler is IHandler<T> h)
            {
                return h;
            }
        }

        throw new ArgumentException($"Handler with type {typeof(T)} not found.", typeof(T).Name);
    }
}