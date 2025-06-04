using Baubit.DI;
namespace ConsoleApp
{
    public class MyConfiguration : AConfiguration
    {
        public MyComponent.Settings MyComponentSettings { get; init; }
    }
}
