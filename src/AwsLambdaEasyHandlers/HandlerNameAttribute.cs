namespace AwsLambdaEasyHandlers;

[AttributeUsage(AttributeTargets.Class)]
public class HandlerNameAttribute : Attribute
{
    public string HandlerName { get; }
    
    public HandlerNameAttribute(string handlerName)
    {
        HandlerName = handlerName;
    }
}