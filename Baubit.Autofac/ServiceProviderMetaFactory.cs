using Autofac.Extensions.DependencyInjection;
using Baubit.DI;
using Microsoft.Extensions.Hosting;

namespace Baubit.Autofac
{
    public sealed class ServiceProviderMetaFactory : IServiceProviderMetaFactory
    {
        private RootModule _rootModule;
        public IHostApplicationBuilder UseConfiguredServiceProviderFactory(IHostApplicationBuilder hostApplicationBuilder)
        {
            if(_rootModule == null) _rootModule = new RootModule(hostApplicationBuilder.Configuration);
            hostApplicationBuilder.ConfigureContainer(new AutofacServiceProviderFactory(), _rootModule.Load);
            return hostApplicationBuilder;
        }
    }
}
