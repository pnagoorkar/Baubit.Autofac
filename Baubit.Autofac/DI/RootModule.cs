using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Baubit.Configuration;
using Baubit.DI;
using FluentResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Baubit.Autofac.DI
{

    public abstract class ARootModuleConfiguration : AConfiguration
    {

    }
    public sealed class RootModuleConfiguration : ARootModuleConfiguration
    {
        public ContainerBuildOptions ContainerBuildOptions { get; init; }
    }

    public abstract class ARootModule<TConfiguration,
                                      TServiceProviderFactory,
                                      TContainerBuilder> : AModule<TConfiguration>, IRootModule where TConfiguration : ARootModuleConfiguration
                                                                                                  where TServiceProviderFactory : IServiceProviderFactory<TContainerBuilder>
                                                                                                  where TContainerBuilder : notnull
    {
        protected ARootModule(ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        protected ARootModule(IConfiguration configuration) : base(configuration)
        {
        }

        protected ARootModule(TConfiguration configuration, List<AModule> nestedModules) : base(configuration, nestedModules)
        {
        }

        public Result<THostApplicationBuilder> UseConfiguredServiceProviderFactory<THostApplicationBuilder>(THostApplicationBuilder hostApplicationBuilder) where THostApplicationBuilder : IHostApplicationBuilder
        {
            hostApplicationBuilder.ConfigureContainer(GetServiceProviderFactory(), GetConfigureAction());
            return hostApplicationBuilder;
        }

        protected abstract TServiceProviderFactory GetServiceProviderFactory();
        protected abstract Action<TContainerBuilder> GetConfigureAction();

    }
    public sealed class RootModule : ARootModule<RootModuleConfiguration, AutofacServiceProviderFactory, ContainerBuilder>, IModule
    {
        public RootModule(ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        public RootModule(IConfiguration configuration) : base(configuration)
        {
        }

        public RootModule(RootModuleConfiguration moduleConfiguration, List<Baubit.DI.AModule> nestedModules) : base(moduleConfiguration, nestedModules)
        {
        }

        public override void Load(ContainerBuilder containerBuilder)
        {
            var baubitModules = new List<Baubit.DI.IModule>();
            var baubitAutofacModules = new List<IModule>();

            this.TrySeparateModuleTypes(baubitModules, baubitAutofacModules);
            baubitAutofacModules.Remove(this);

            var services = new ServiceCollection();
            baubitModules.ForEach(module => module.Load(services));
            containerBuilder.Populate(services);
            baubitAutofacModules.ForEach(module => module.Load(containerBuilder));
        }

        protected override Action<ContainerBuilder> GetConfigureAction() => Load;

        protected override AutofacServiceProviderFactory GetServiceProviderFactory() => new AutofacServiceProviderFactory(Configuration.ContainerBuildOptions);
    }

    public static class ModuleExtensions
    {
        public static bool TrySeparateModuleTypes<TModule>(this TModule module,
                                                           List<Baubit.DI.IModule> baubitModules,
                                                           List<IModule> baubitAutofacModules) where TModule : Baubit.DI.IModule
        {
            if (baubitModules == null) baubitModules = new List<Baubit.DI.IModule>();
            if (baubitAutofacModules == null) baubitAutofacModules = new List<IModule>();

            if (module is IModule autofacModule)
            {
                baubitAutofacModules.Add(autofacModule);
            }
            else
            {
                baubitModules.Add(module);
            }

            foreach(var nestedModule in module.NestedModules)
            {
                nestedModule.TrySeparateModuleTypes(baubitModules, baubitAutofacModules);
            }

            return true;
        }
    }
}
