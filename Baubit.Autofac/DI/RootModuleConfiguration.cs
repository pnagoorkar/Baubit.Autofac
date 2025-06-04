using Autofac.Builder;
using Baubit.DI;

namespace Baubit.Autofac.DI
{
    public sealed class RootModuleConfiguration : ARootModuleConfiguration
    {
        public ContainerBuildOptions ContainerBuildOptions { get; init; }
    }
}
