using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    public class MyComponent
    {
        public Settings InitSettings { get; set; }
        public MyComponent(Settings settings, ILogger<MyComponent> logger)
        {
            InitSettings = settings;
        }

        public class Settings
        {
            public string SomeStringValue { get; init; }
        }
    }
}
