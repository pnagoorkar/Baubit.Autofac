﻿using Baubit.Configuration;
using Baubit.DI;
using Baubit.Store;
using System.Reflection;
using System.Text.Json;

namespace Baubit.Autofac.Test.AModule
{
    public class Test
    {
        [Theory]
        [InlineData($"modules.json")]
        public async void CanLoadModuleConfiguration_FromMetaConfiguration_UsingASingleJsonString(string jsonFile)
        {
            var readResult = await Assembly.GetExecutingAssembly().ReadResource($"{this.GetType().Namespace}.{jsonFile}");
            Assert.True(readResult.IsSuccess);
            var metaConfiguration = new MetaConfiguration { RawJsonStrings = [readResult.Value] };
            var modules = metaConfiguration.Load().GetNestedModules();
            Assert.NotEmpty(modules);
            Assert.Single(modules);
            Assert.NotNull(modules.First().ModuleConfiguration);
        }
        [Theory]
        [InlineData($"modules_ThreeLevelDeep.json")]
        public async void ModulesAreSerializable(string jsonFile)
        {
            var readResult = await Assembly.GetExecutingAssembly().ReadResource($"{this.GetType().Namespace}.{jsonFile}");

            Assert.True(readResult.IsSuccess);
            var metaConfiguration = new MetaConfiguration { RawJsonStrings = [readResult.Value] };
            var modules = metaConfiguration.Load().GetNestedModules();

            Assert.Equal(3, modules.Sum(m => m.CountTotalNodes()));
            Assert.Equal(3, modules.Max(m => m.CountNodesInDeepestSubgraph()));

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var jsonString = JsonSerializer.Serialize(new { Modules = modules }, options);

            var reloadedModules = new MetaConfiguration { RawJsonStrings = [jsonString] }.Load().GetNestedModules();

            Assert.Equal(3, reloadedModules.Sum(m => m.CountTotalNodes()));
            Assert.Equal(3, reloadedModules.Max(m => m.CountNodesInDeepestSubgraph()));

            var jsonString2 = JsonSerializer.Serialize(new { Modules = modules }, options);

            Assert.Equal(jsonString, jsonString2);
        }
    }
}

