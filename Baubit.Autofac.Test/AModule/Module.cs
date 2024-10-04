using Baubit.Configuration;
using Microsoft.Extensions.Configuration;

namespace Baubit.Autofac.Test.AModule
{
    public class Module : AModule<ModuleConfiguration>
    {
        public Module(ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        public Module(IConfiguration configuration) : base(configuration)
        {
        }

        public Module(ModuleConfiguration moduleConfiguration, List<Autofac.AModule> nestedModules) : base(moduleConfiguration, nestedModules)
        {
        }
    }
}
