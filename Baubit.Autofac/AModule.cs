using Autofac;
using Baubit.DI;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using Baubit.Configuration;
using FluentValidation;

namespace Baubit.Autofac
{
    [JsonConverter(typeof(ModuleJsonConverter))]
    public abstract class AModule : Module
    {
        [JsonIgnore]
        public DI.AConfiguration Configuration { get; init; }
        [JsonIgnore]
        public IReadOnlyList<AModule> NestedModules { get; init; }

        public AModule(DI.AConfiguration moduleConfiguration, List<AModule> nestedModules)
        {
            Configuration = moduleConfiguration;
            NestedModules = nestedModules.AsReadOnly();
            OnInitialized();
        }
        /// <summary>
        /// Called by the constructor in <see cref="AModule"/> after all construction activities.
        /// Override this method to perform construction in child types.
        /// </summary>
        protected virtual void OnInitialized()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            foreach (var module in NestedModules)
            {
                builder.RegisterModule(module);
            }
            base.Load(builder);
        }
    }
    public abstract class AModule<TConfiguration> : AModule where TConfiguration : DI.AConfiguration
    {
        public new TConfiguration? Configuration
        {
            get => (TConfiguration?)base.Configuration;
        }
        protected AModule(ConfigurationSource configurationSource) : this(configurationSource.Load())
        {

        }

        protected AModule(IConfiguration configuration) : this(configuration.Load<TConfiguration>(),
                                                               configuration.GetNestedModules<AModule>().ToList())
        {

        }

        protected AModule(TConfiguration moduleConfiguration,
                          List<AModule> nestedModules) : base(moduleConfiguration, nestedModules)
        {

        }
    }

    public static class ModuleExtensions
    {
        public static int CountTotalNodes(this AModule module)
        {
            return module == null ? 0 : 1 + module.NestedModules.Sum(m => m.CountTotalNodes());
        }
        public static int CountNodesInDeepestSubgraph(this AModule module)
        {
            return module == null ? 0 : 1 + (module.NestedModules.Any() ? module.NestedModules.Max(m => m.CountNodesInDeepestSubgraph()) : 0);
        }

        public static string AddIndentation(this string jsonString)
        {
            using var jsonDocument = JsonDocument.Parse(jsonString);
            using var memoryStream = new MemoryStream();
            var writerOptions = new JsonWriterOptions { Indented = true };

            using (var jsonWriter = new Utf8JsonWriter(memoryStream, writerOptions))
            {
                jsonDocument.WriteTo(jsonWriter);
            }

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static TModule Validate<TModule>(this TModule module) where TModule : AModule
        {
            if (AModuleValidator<TModule>.CurrentValidators.TryGetValue(module.Configuration.ModuleValidatorKey, out var validator))
            {
                var validationResult = validator.Validate(module);
                if (validationResult != null && !validationResult.IsValid)
                {
                    throw new ValidationException($"Invalid module !{string.Join(Environment.NewLine, validationResult.Errors)}");
                }
            }
            return module;
        }
    }

    public class ModuleJsonConverter : JsonConverter<AModule>
    {
        public override AModule? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, AModule value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteStringValue(value.GetType().AssemblyQualifiedName);
            writer.WritePropertyName("parameters");
            writer.WriteStartObject();

            var serializedModuleConfiguration = value.Configuration.SerializeJson(options);

            var serializedNestedModules = value.NestedModules.Select(nestedModule => JsonSerializer.Serialize(nestedModule, options));

            writer.WritePropertyName(nameof(AModule.Configuration));

            if (serializedNestedModules.Any())
            {
                serializedModuleConfiguration = $"{serializedModuleConfiguration.TrimEnd('}')},\"modules\":[{string.Join(",", serializedNestedModules)}]{"}"}";
            }
            writer.WriteRawValue(serializedModuleConfiguration);

            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}