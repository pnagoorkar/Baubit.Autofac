using Autofac.Extensions.DependencyInjection;
using Baubit.DI;
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
        public IHostApplicationBuilder UseConfiguredServiceProviderFactory(IHostApplicationBuilder hostApplicationBuilder)
        {
            hostApplicationBuilder.ConfigureContainer(new AutofacServiceProviderFactory(), _rootModule.Load);
            return hostApplicationBuilder;
        }
    }
}
