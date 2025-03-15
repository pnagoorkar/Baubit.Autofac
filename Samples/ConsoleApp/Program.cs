using Baubit.DI;
using Microsoft.Extensions.Hosting;

var hostAppBuilder = new HostApplicationBuilder();
hostAppBuilder.UseConfiguredServiceProviderFactory();
var host = hostAppBuilder.Build();
await host.RunAsync();