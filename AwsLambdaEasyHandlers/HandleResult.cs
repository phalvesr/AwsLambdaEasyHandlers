namespace AwsLambdaEasyHandlers;

public class HandleResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Exception? Error { get; }

    private HandleResult(bool success, Exception? error)
    {
        IsSuccess = success;
        Error = error;
    }
    
    public static HandleResult SuccessResult() => new(true, null);
    public static HandleResult FailureResult(Exception error) => new(false, error);
}