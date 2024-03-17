namespace AwsLambdaEasyHandlers;

public interface IHandlerContext
{
    public IHandler<T> GetHandler<T>();
}