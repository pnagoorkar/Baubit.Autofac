using Autofac;
using Baubit.Configuration;
using FluentResults;
using Microsoft.Extensions.Configuration;

namespace Baubit.Autofac.DI
{
    public abstract class AConfiguration : Baubit.DI.AConfiguration
    {
    }

    public static class ConfigurationExtensions
    {
        public static Result<IContainer> Load(this IConfiguration configuration)
        {
            return Result.Try(() =>
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.AddFrom(configuration);
                return containerBuilder.Build();
            });
        }
        public static Result<ContainerBuilder> AddFrom(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            return Result.Try(() =>
            {
                var rootModule = new RootModule(configuration);
                rootModule.Load(containerBuilder);
                return containerBuilder;
            });
        }
        public static Result<ContainerBuilder> AddFrom(this ContainerBuilder containerBuilder, ConfigurationSource configurationSource) => containerBuilder.AddFrom(configurationSource.Build().Value);
    }
}
