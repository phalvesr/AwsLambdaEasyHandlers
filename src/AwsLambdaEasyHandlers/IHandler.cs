namespace AwsLambdaEasyHandlers;

public interface IHandler<in THandle> : IBaseHandler
{
    public HandleResult Handle(THandle input);
}