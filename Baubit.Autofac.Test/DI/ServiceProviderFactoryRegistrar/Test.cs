using Autofac;
using Baubit.Autofac.DI;
using Baubit.Autofac.Test.DI.Setup;
using Baubit.Configuration;
using Baubit.Reflection;
using FluentResults;
using System.Runtime.InteropServices;

namespace Baubit.Autofac.Test.DI.ServiceProviderFactoryRegistrar
{
    public class Test
    {
        private const string UserSecretsId = "0657aef1-6dc5-48f1-8ae4-172674291be1";

        private static readonly string SecretsPath = GetUserSecretsPath(UserSecretsId);

        private static string GetUserSecretsPath(string userSecretsId)
        {
            string basePath;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                basePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Microsoft", "UserSecrets");
            }
            else // Linux and macOS
            {
                basePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    ".microsoft", "usersecrets");
            }

            return Path.Combine(basePath, userSecretsId, "secrets.json");
        }

        [Theory]
        [InlineData("config.json")]
        public void CanLoadModulesFromJson(string fileName)
        {
            var configurationSource = new ConfigurationSource { EmbeddedJsonResources = [$"{this.GetType().Assembly.GetName().Name};DI.ServiceProviderFactoryRegistrar.{fileName}"] };

            var component = configurationSource.Build()
                                               .Bind(config => config.Load())
                                               .Bind(container => Result.Try(() => container.Resolve<Component>())).Value;

            Assert.NotNull(component);
            Assert.False(string.IsNullOrEmpty(component.SomeString));
        }

        [Theory]
        [InlineData("configWithSecrets.json", "secrets.json")]
        public void CanLoadModulesWithSecretsFromJson(string fileName, string secretsFile)
        {
            var readResult = this.GetType().Assembly.ReadResource($"{this.GetType().Namespace}.{secretsFile}").GetAwaiter().GetResult();
            Assert.True(readResult.IsSuccess);

            Directory.CreateDirectory(Path.GetDirectoryName(SecretsPath)!);

            File.WriteAllText(SecretsPath, readResult.Value);

            var configurationSource = new ConfigurationSource { EmbeddedJsonResources = [$"{this.GetType().Assembly.GetName().Name};DI.ServiceProviderFactoryRegistrar.{fileName}"] };

            var component = configurationSource.Build()
                                               .Bind(config => config.Load())
                                               .Bind(container => Result.Try(() => container.Resolve<Component>())).Value;

            Assert.NotNull(component);
            Assert.False(string.IsNullOrEmpty(component.SomeString));
            Assert.False(string.IsNullOrEmpty(component.SomeSecretString));
        }

        [Theory]
        [InlineData("configWithEmptyConfiguration.json")]
        public void CanLoadModulesFromJsonWhenConfigurationIsEmpty(string fileName)
        {
            var configurationSource = new ConfigurationSource { EmbeddedJsonResources = [$"{this.GetType().Assembly.GetName().Name};DI.ServiceProviderFactoryRegistrar.{fileName}"] };

            var component = configurationSource.Build()
                                               .Bind(config => config.Load())
                                               .Bind(container => Result.Try(() => container.Resolve<Component>())).Value;

            Assert.NotNull(component);
        }

        [Theory]
        [InlineData("configWithEmptyConfigurationSource.json")]
        public void CanLoadModulesFromJsonWhenConfigurationSourceIsEmpty(string fileName)
        {
            var configurationSource = new ConfigurationSource { EmbeddedJsonResources = [$"{this.GetType().Assembly.GetName().Name};DI.ServiceProviderFactoryRegistrar.{fileName}"] };

            var component = configurationSource.Build()
                                               .Bind(config => config.Load())
                                               .Bind(container => Result.Try(() => container.Resolve<Component>())).Value;

            Assert.NotNull(component);
        }
    }
}
