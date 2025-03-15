using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Baubit.Autofac.DI
{
    public interface IModule
    {
        void Load(ContainerBuilder containerBuilder);
    }
    public abstract class AModule<TConfiguration> : Baubit.DI.AModule<TConfiguration>, IModule where TConfiguration : AConfiguration
    {
        protected AModule(Configuration.ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        protected AModule(IConfiguration configuration) : base(configuration)
        {
        }

        protected AModule(TConfiguration configuration, List<Baubit.DI.AModule> nestedModules) : base(configuration, nestedModules)
        {
        }

        [Obsolete("Call the overload with ContainerBuilder", error: true)]
        public new void Load(IServiceCollection services)
        {
            throw new NotSupportedException();
        }

        public virtual void Load(ContainerBuilder containerBuilder)
        {
            foreach (var module in NestedModules)
            {
                if (module is IModule autofacModule)
                {
                    autofacModule.Load(containerBuilder);
                }
                else
                {
                    var services = new ServiceCollection();
                    module.Load(services);
                    containerBuilder.Populate(services);
                }
            }
        }
    }
}
