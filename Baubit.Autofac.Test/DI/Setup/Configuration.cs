namespace Baubit.Autofac.Test.DI.Setup
{
    public class Configuration : Baubit.Autofac.DI.AConfiguration
    {
        public string SomeString { get; init; }
        public string SomeSecretString { get; init; }
    }
}
