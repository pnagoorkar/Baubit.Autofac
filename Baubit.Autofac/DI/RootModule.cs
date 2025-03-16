using Autofac;
using Autofac.Extensions.DependencyInjection;
using Baubit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Baubit.Autofac.DI
{
    internal sealed class RootModuleConfiguration : AConfiguration
    {

    }
    internal sealed class RootModule : AModule<RootModuleConfiguration>
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
