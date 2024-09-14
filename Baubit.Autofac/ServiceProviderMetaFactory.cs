using Autofac.Extensions.DependencyInjection;
using Baubit.DI;
using Microsoft.Extensions.Hosting;

namespace Baubit.Autofac
{
    public sealed class ServiceProviderMetaFactory : IServiceProviderMetaFactory
    {
        public IHostApplicationBuilder UseConfiguredServiceProviderFactory(IHostApplicationBuilder hostApplicationBuilder)
        {
            hostApplicationBuilder.ConfigureContainer(new AutofacServiceProviderFactory());
            return hostApplicationBuilder;
        }
    }
}
