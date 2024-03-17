using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AwsLambdaEasyHandlers.UnitTests;

public class HandlerContextDefaultTests
{
    private readonly HandlerContextDefault _uut;

    public HandlerContextDefaultTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IBaseHandler, HandlerForUnitTests>()
            .BuildServiceProvider();
        
        _uut = new HandlerContextDefault(serviceProvider);
    }
    
    
    [Fact]
    public void GetHandlerByName_ShouldReturnHandlerWithTheGivenKey()
    {
        // Arrange & Act
        var result = _uut.GetHandler<string>();
        
        
        // Assert
        result.Should().BeAssignableTo<IHandler<string>>();
    }
    
    [Fact]
    public void GetHandlerByName_ShouldThrowArgumentExceptionWhenHandlerWithTheGivenKeyDoesNotExist()
    {
        // Arrange & Act
        var act = () => _uut.GetHandler<int>();
        
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetHandler_ShouldRespectTransientLifetime()
    {
        // Arrange
        var serviceCollection = new ServiceCollection()
            .AddTransient<IBaseHandler, HandlerForUnitTests>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var uut = new HandlerContextDefault(serviceProvider);

        
        // Act
        var handler00 = uut.GetHandler<string>();
        var handler01 = uut.GetHandler<string>();

        
        // Assert
        ReferenceEquals(handler00, handler01).Should().BeFalse();
    }
    
    [Fact]
    public void GetHandler_ShouldRespectScopedLifetime()
    {
        // Arrange
        var serviceCollection = new ServiceCollection()
            .AddScoped<IBaseHandler, HandlerForUnitTests>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        
        var uutWithScope0 = new HandlerContextDefault(scope.ServiceProvider);
        var uutWithScope1 = new HandlerContextDefault(scope.ServiceProvider);
        
        
        // Act
        var handler00 = uutWithScope0.GetHandler<string>();
        var handler01 = uutWithScope1.GetHandler<string>();

        
        // Assert
        ReferenceEquals(handler00, handler01).Should().BeTrue();
    }
    
    [Fact]
    public void GetHandler_ShouldReturnTwoDifferentInstancesForTwoDifferentScopesWhenHandlerIsRegisterAsScoped()
    {
        // Arrange
        var serviceCollection = new ServiceCollection()
            .AddScoped<IBaseHandler, HandlerForUnitTests>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        using var scope0 = serviceProvider.CreateScope();
        using var scope1 = serviceProvider.CreateScope();
        
        var uutWithScope0 = new HandlerContextDefault(scope0.ServiceProvider);
        var uutWithScope1 = new HandlerContextDefault(scope1.ServiceProvider);
        
        
        // Act
        var handler00 = uutWithScope0.GetHandler<string>();
        var handler01 = uutWithScope1.GetHandler<string>();

        
        // Assert
        ReferenceEquals(handler00, handler01).Should().BeFalse();
    }
    
    [Fact]
    public void GetHandler_ShouldRespectSingletonLifetime()
    {
        // Arrange
        var serviceCollection = new ServiceCollection()
            .AddSingleton<IBaseHandler, HandlerForUnitTests>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var uut = new HandlerContextDefault(serviceProvider);

        
        // Act
        var handler00 = uut.GetHandler<string>();
        var handler01 = uut.GetHandler<string>();

        
        // Assert
        ReferenceEquals(handler00, handler01).Should().BeTrue();
    }
    
    private class HandlerForUnitTests : IHandler<string>
    {
        public HandleResult Handle(string input)
        {
            return HandleResult.SuccessResult();
        }
    }
}