using Autofac.Extensions.DependencyInjection;
using Baubit.DI;
using FluentResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Baubit.Autofac.DI
{
    public sealed class ServiceProviderFactoryRegistrar : IServiceProviderFactoryRegistrar
    {
        private readonly RootModule _rootModule;
        public ServiceProviderFactoryRegistrar(IConfiguration configuration)
        {
            _rootModule = new RootModule(configuration);
        }
        public Result<THostApplicationBuilder> UseConfiguredServiceProviderFactory<THostApplicationBuilder>(THostApplicationBuilder hostApplicationBuilder) where THostApplicationBuilder : IHostApplicationBuilder
        {
            hostApplicationBuilder.ConfigureContainer(new AutofacServiceProviderFactory(), _rootModule.Load);
            return hostApplicationBuilder;
        }
    }
}
