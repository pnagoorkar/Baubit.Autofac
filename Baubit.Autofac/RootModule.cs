using Autofac;
using Baubit.Configuration;
using Microsoft.Extensions.Configuration;

namespace Baubit.Autofac
{
    public sealed class RootModuleConfiguration : DI.AConfiguration
    {

    }
    public sealed class RootModule : AModule<RootModuleConfiguration>
    {
        public RootModule(ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        public RootModule(IConfiguration configuration) : base(configuration)
        {
        }

        public RootModule(RootModuleConfiguration moduleConfiguration, List<AModule> nestedModules) : base(moduleConfiguration, nestedModules)
        {
        }

        public new void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}
