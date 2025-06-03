using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Baubit.Autofac.DI
{
    public interface IModule : Baubit.DI.IModule
    {
        [Obsolete("Call the overload with ContainerBuilder", error: true)]
        new void Load(IServiceCollection services) => throw new NotSupportedException();

        void Load(ContainerBuilder containerBuilder);
    }
}
