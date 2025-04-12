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
            return Result.Try(() => new ContainerBuilder())
                         .Bind(containerBuilder => containerBuilder.AddFrom(configuration))
                         .Bind(containerBuilder => Result.Try(() => containerBuilder.Build()));
        }
        public static Result<ContainerBuilder> AddFrom(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            return Result.Try(() => new RootModule(configuration))
                         .Bind(rootModule => Result.Try(() => rootModule.Load(containerBuilder)))
                         .Bind(() => Result.Ok(containerBuilder));
        }
        public static Result<ContainerBuilder> AddFrom(this ContainerBuilder containerBuilder, ConfigurationSource configurationSource) => configurationSource.Build().Bind(containerBuilder.AddFrom);
    }
}
