using Autofac;
using Autofac.Extensions.DependencyInjection;
using Baubit.Configuration;
using Baubit.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    public class MyModule : AModule<MyConfiguration>, Baubit.Autofac.DI.IModule
    {
        public MyModule(ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        public MyModule(IConfiguration configuration) : base(configuration)
        {
        }

        public MyModule(MyConfiguration configuration, List<AModule> nestedModules, List<IConstraint> constraints) : base(configuration, nestedModules, constraints)
        {
        }

        public void Load(ContainerBuilder containerBuilder)
        {
            var services = new ServiceCollection();
            services.AddHostedService<MyHostedService>();
            containerBuilder.RegisterType<MyComponent>()
                            .WithParameters([new TypedParameter(typeof(MyComponent.Settings), Configuration.MyComponentSettings)])
                            .SingleInstance();
            containerBuilder.Populate(services);
        }
    }
}
