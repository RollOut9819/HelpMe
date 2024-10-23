using Microsoft.Extensions.Configuration;

namespace HelpMi.Infrastructure
{
    public static class AppSetting
    {

        public static IConfigurationRoot GetAppSettingJsonContent()
        {
            // Recupera la configurazione dal file appsettings.json
            IConfigurationRoot appsettingJsonContent = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return appsettingJsonContent;
        }
    }
}