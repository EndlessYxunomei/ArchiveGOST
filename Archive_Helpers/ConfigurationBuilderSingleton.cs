using Microsoft.Extensions.Configuration;

namespace Archive_Helpers
{
    public sealed class ConfigurationBuilderSingleton
    {
        private static ConfigurationBuilderSingleton _instance = null;
        private static readonly object instanceLock = new();
        private static IConfigurationRoot _configuration;
        public ConfigurationBuilderSingleton()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
        }
        public static ConfigurationBuilderSingleton Instance
        {
            get
            {
                lock (instanceLock)
                {
                    _instance ??= new ConfigurationBuilderSingleton();
                }
                return _instance;
            }
        }
        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                if (_configuration == null) {
                    _ = Instance;
                }
                return _configuration;
            }
        }
    }
}