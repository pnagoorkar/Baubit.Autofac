namespace Baubit.Autofac.Test.DI.Setup
{
    public class Configuration : Baubit.DI.AConfiguration
    {
        public string SomeString { get; init; }
        public string SomeSecretString { get; init; }
    }
}
