using FluentAssertions;

namespace AwsLambdaEasyHandlers.UnitTests;

public class HandleResultTests
{
    [Fact]
    public void SuccessResult_ShouldReturnSuccessResult()
    {
        // Arrange & Act
        var result = HandleResult.SuccessResult();
        
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeNull();
    }
    
    [Fact]
    public void FailureResult_ShouldReturnFailureResult()
    {
        // Arrange
        var exception = new Exception();
        
        
        // Act
        var result = HandleResult.FailureResult(exception);
        
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(exception);
    }
}