using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Baubit.Autofac.DI
{
    public interface IModule : Baubit.DI.IModule
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

        public abstract void Load(ContainerBuilder containerBuilder);
    }
}
