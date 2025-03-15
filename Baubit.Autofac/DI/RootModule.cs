﻿using Autofac;
using Baubit.Configuration;
using Microsoft.Extensions.Configuration;

namespace Baubit.Autofac.DI
{
    internal sealed class RootModuleConfiguration : AConfiguration
    {

    }
    internal sealed class RootModule : AModule<RootModuleConfiguration>
    {
        public RootModule(ConfigurationSource configurationSource) : base(configurationSource)
        {
        }

        public RootModule(IConfiguration configuration) : base(configuration)
        {
        }

        public RootModule(RootModuleConfiguration moduleConfiguration, List<Baubit.DI.AModule> nestedModules) : base(moduleConfiguration, nestedModules)
        {
        }

        public new void Load(ContainerBuilder containerBuilder)
        {
            base.Load(containerBuilder);
        }
    }
}
