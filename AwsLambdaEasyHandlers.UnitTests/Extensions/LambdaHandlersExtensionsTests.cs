using System.Reflection;
using AwsLambdaEasyHandlers.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AwsLambdaEasyHandlers.UnitTests.Extensions;

public class LambdaHandlersExtensionsTests
{
    [Fact]
    public void AddHandlersFromAssembly_ShouldAddHandlersFromTheGivenAssemblyToTheContextWithTheProvidedNameAsKey()
    {
        // Arrange
        var assembly = new FakeAssembly();
        var serviceCollection = new ServiceCollection();
        
        
        // Act
        serviceCollection.AddContextWithHandlers(assembly);
        
        var provider = serviceCollection.BuildServiceProvider();
        
        var handlerContext = provider.GetRequiredService<IHandlerContext>();
        
        
        // Assert
        var handler00 = handlerContext.GetHandler<string>();
        var handler01 = handlerContext.GetHandler<int>();
        
        handler00.Should().NotBeNull();
        handler01.Should().NotBeNull();
    }
    
    [Fact]
    public void AddHandlersFromAssembly_ShouldAddHandlersFromTheGivenAssemblyToTheContextAsScopedByDefault()
    {
        // Arrange
        var assembly = new FakeAssembly();
        var serviceCollection = new ServiceCollection();
        
        
        // Act
        serviceCollection.AddContextWithHandlers(assembly);
        
        var handler00 = serviceCollection.Single(x => x.ImplementationType == typeof(FakeHandler00));
        var handler01 = serviceCollection.Single(x => x.ImplementationType == typeof(FakeHandler01));

        // Assert
        handler00.Lifetime.Should().Be(ServiceLifetime.Scoped);
        handler01.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }
    
    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void AddHandlersFromAssembly_ShouldAddHandlersFromTheGivenAssemblyToTheContextWithTheProvidedLifetime(
        ServiceLifetime lifetime)
    {
        // Arrange
        var assembly = new FakeAssembly();
        var serviceCollection = new ServiceCollection();
        
        
        // Act
        serviceCollection.AddContextWithHandlers(assembly, lifetime);
        
        var handler00 = serviceCollection.Single(x => x.ImplementationType == typeof(FakeHandler00));
        var handler01 = serviceCollection.Single(x => x.ImplementationType == typeof(FakeHandler01));

        
        // Assert
        handler00.Lifetime.Should().Be(lifetime);
        handler01.Lifetime.Should().Be(lifetime);
    }
    
    
    [Fact]
    public void AddHandlers_ShouldAddHandlersFromTheGivenAssemblyToTheContextWithTheProvidedLifetime()
    {
        // Arrange
        var assembly = new FakeAssembly();
        var serviceCollection = new ServiceCollection();
        
        
        // Act
        serviceCollection.AddHandlers(assembly);
        
        var handler00 = serviceCollection.Single(x => x.ImplementationType == typeof(FakeHandler00));
        var handler01 = serviceCollection.Single(x => x.ImplementationType == typeof(FakeHandler01));

        
        // Assert
        handler00.Lifetime.Should().Be(ServiceLifetime.Scoped);
        handler01.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }
    
    [Theory]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    [InlineData(ServiceLifetime.Singleton)]
    public void AddHandlers_ShouldAddHandlersFromTheGivenAssemblyToTheContextWithTheExpectedLifetime(ServiceLifetime lifetime)
    {
        // Arrange
        var assembly = new FakeAssembly();
        var serviceCollection = new ServiceCollection();
        
        
        // Act
        serviceCollection.AddHandlers(assembly, lifetime);
        
        var handler00 = serviceCollection.Single(x => x.ImplementationType == typeof(FakeHandler00));
        var handler01 = serviceCollection.Single(x => x.ImplementationType == typeof(FakeHandler01));

        
        // Assert
        handler00.Lifetime.Should().Be(lifetime);
        handler01.Lifetime.Should().Be(lifetime);
    }
    
    private class FakeAssembly : Assembly
    {
        public override Type[] GetTypes()
        {
            return new Type[]
            {
                typeof(FakeHandler00),
                typeof(FakeHandler01),
                typeof(string),
                typeof(int),
                typeof(object)
            };
        }
    }

    [HandlerName("FakeHandler00")]
    private class FakeHandler00 : IHandler<string>
    {
        public HandleResult Handle(string input)
        {
            return HandleResult.SuccessResult();
        }
    }
    
    [HandlerName("FakeHandler01")]
    private class FakeHandler01 : IHandler<int>
    {
        public HandleResult Handle(int input)
        {
            return HandleResult.SuccessResult();
        }
    }
}