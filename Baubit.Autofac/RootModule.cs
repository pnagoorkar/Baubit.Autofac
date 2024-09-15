using Autofac;
using Baubit.Configuration;
using Baubit.DI;
using Microsoft.Extensions.Configuration;

namespace Baubit.Autofac
{
    public sealed class RootModuleConfiguration : AModuleConfiguration
    {

    }
    public sealed class RootModule : AModule<RootModuleConfiguration>
    {
        public RootModule(MetaConfiguration metaConfiguration) : base(metaConfiguration)
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
