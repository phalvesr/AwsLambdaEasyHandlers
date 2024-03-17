namespace AwsLambdaEasyHandlers;

public interface IAsyncHandler<in THandle> : IBaseHandler
{
    public Task<HandleResult> HandleAsync(THandle input, CancellationToken cancellationToken);
}