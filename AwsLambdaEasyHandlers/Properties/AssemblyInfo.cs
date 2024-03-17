using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("AwsLambdaEasyHandlers.UnitTests")]
// Make internals visible to DynamicProxyGenAssembly2 for mock library
[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2")]