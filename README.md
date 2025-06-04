# Baubit.Autofac

[![NuGet](https://img.shields.io/nuget/v/Baubit.Autofac.svg)](https://www.nuget.org/packages/Baubit.Autofac)

[Autofac](https://github.com/autofac/Autofac) support for [Baubit](https://github.com/pnagoorkar/Baubit)

## Features

- **AutofacServiceProviderFactory:** Use ContainerBuilder to add services to the service provider.
- **Seamless Integration:** Reuse existing Baubit modules (extending Baubit.DI.AModule<>) as nested modules for a Baubit.Autofac.DI.AModule<>.

## 🚀 Getting Started
```bash
dotnet add package Baubit.Autofac
```

### 📦 Defining a Module
Similar to Baubit, create a Module and associated configuration

```csharp
public class MyConfiguration : AConfiguration
{
    public string MyStringProperty { get; set; }
}

public class MyModule : AModule<MyConfiguration>
{
    public MyModule(ConfigurationSource configurationSource) : base(configurationSource) { }
    public MyModule(IConfiguration configuration) : base(configuration) { }
    public MyModule(MyConfiguration configuration, List<AModule> nestedModules, List<Baubit.DI.IConstraint> constraints) : base(configuration, nestedModules, constraints) { }

    public override void Load(ContainerBuilder containerBuilder)
    {
        containerBuilder.Register(context => new Component(Configuration.SomeString, Configuration.SomeSecretString))
                        .SingleInstance();
        //register other services as needed
    }
}
```
## 🔍 Example Usage

### Bootstrapping the Application

#### Using HostApplicationBuilder

```csharp
await Host.CreateApplicationBuilder()
          .UseConfiguredServiceProviderFactory()
          .Build()
          .RunAsync();
```

#### Using WebApplicationBuilder

```csharp
var webApp = WebApplication.CreateBuilder()
                           .UseConfiguredServiceProviderFactory()
                           .Build();

// use HTTPS, HSTS, CORS, Auth and other middleware
// map endpoints

await webApp.RunAsync();
```
appsettings.json
```json

{
  "rootModule": {
    "type": "Baubit.Autofac.DI.RootModule, Baubit.Autofac",
    "configurationSource": {
      "embeddedJsonResources": [ "MyApp;myConfig.json" ]
    }
  }
}

```
myConfig.json

```json
{
  "modules": [
    {
      "type": "MyProject.MyModule, MyProject",
      "configuration": {
          "myStringProperty" : "some string value"
          }
     }
  ]
}
```

## 📄 License
Baubit is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.
