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

        public new void Load(ContainerBuilder containerBuilder, IServiceCollection services = null)
        {
            if (services == null) services = new ServiceCollection();
            base.Load(containerBuilder, services);
            containerBuilder.Populate(services);
        }

        public void Load(ContainerBuilder containerBuilder) => Load(containerBuilder, null);
    }
}
