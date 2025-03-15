using Autofac;
using Baubit.Autofac.DI;
using Baubit.Configuration;
using Microsoft.Extensions.Configuration;

namespace Baubit.Autofac.Test.DI.Setup
{
    public class Module : AModule<Configuration>
    {
        public Module(ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        public Module(IConfiguration configuration) : base(configuration)
        {
        }

        public Module(Configuration configuration, List<Baubit.DI.AModule> nestedModules) : base(configuration, nestedModules)
        {
        }

        public override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(context => new Component(Configuration.SomeString, Configuration.SomeSecretString))
                            .SingleInstance();
            base.Load(containerBuilder);
        }
    }
}
