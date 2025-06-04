using Autofac;
using Baubit.DI;
using Baubit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Baubit.Autofac.Test.DI.Setup
{
    public class Module : AModule<Configuration>, Baubit.Autofac.DI.IModule
    {
        public Module(ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        public Module(IConfiguration configuration) : base(configuration)
        {
        }

        public Module(Configuration configuration, List<Baubit.DI.AModule> nestedModules, List<Baubit.DI.IConstraint> constraints) : base(configuration, nestedModules, constraints)
        {
        }

        public void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(context => new Component(Configuration.SomeString, Configuration.SomeSecretString))
                            .SingleInstance();
        }
    }
}
