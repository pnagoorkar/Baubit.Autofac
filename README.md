# Baubit.Autofac

## Introduction
Autofac support for [Baubit](https://github.com/pnagoorkar/Baubit)

## 🚀 Getting Started
```
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
    public MyModule(MyConfiguration configuration, List<AModule> nestedModules) : base(configuration, nestedModules) { }

    public override void Load(ContainerBuilder containerBuilder)
    {
        containerBuilder.Register(context => new Component(Configuration.SomeString, Configuration.SomeSecretString))
                        .SingleInstance();
        //register other services as needed
        base.Load(services);
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

// use CORS, Auth and other middleware
// map endpoints

await webApp.RunAsync();
```
appsettings.json
```json

{
  "serviceProviderFactory": {
    "type": "Baubit.Autofac.DI.ServiceProviderFactoryRegistrar, Baubit.Autofac",
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
Baubit is licensed under the **Apache-2.0 License**. See the [LICENSE](LICENSE) file for details.