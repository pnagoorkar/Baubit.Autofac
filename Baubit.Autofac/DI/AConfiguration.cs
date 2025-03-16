using Autofac;
using Baubit.Configuration;
using Microsoft.Extensions.Configuration;

namespace Baubit.Autofac.DI
{
    public abstract class AConfiguration : Baubit.DI.AConfiguration
    {
    }

    public static class ConfigurationExtensions
    {
        public static IContainer Load(this IConfiguration configuration)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.AddFrom(configuration);
            return containerBuilder.Build();
        }
        public static ContainerBuilder AddFrom(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            var rootModule = new RootModule(configuration);
            rootModule.Load(containerBuilder);
            return containerBuilder;
        }
        public static ContainerBuilder AddFrom(this ContainerBuilder containerBuilder, ConfigurationSource configurationSource) => containerBuilder.AddFrom(configurationSource.Build());
    }
}
