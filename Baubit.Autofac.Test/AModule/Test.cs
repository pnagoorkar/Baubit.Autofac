using Baubit.Configuration;
using Baubit.DI;
using System.Reflection;

namespace Baubit.Autofac.Test.AModule
{
    public class Test
    {
        [Fact]
        public async void ModulesCanBeLoadedDynamically()
        {
            var readResult = await Baubit.Resource
                                         .Operations
                                         .ReadEmbeddedResource
                                         .RunAsync(new Resource.ReadEmbeddedResource.Context($"{this.GetType().Namespace}.modules.json", Assembly.GetExecutingAssembly()));
            Assert.True(readResult.Success);
            var metaConfiguration = new MetaConfiguration { RawJsonStrings = [readResult.Value] };
            var modules = metaConfiguration.Load().GetNestedModules();
            Assert.NotEmpty(modules);
            Assert.Single(modules);
            Assert.NotNull(modules.First().ModuleConfiguration);
        }
    }
}

