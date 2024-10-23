using Microsoft.Extensions.Configuration;

namespace HelpMi.Infrastructure
{
    public static class ConnectionStrings
    {
        /// <summary>
        /// Metodo per ottenere la stringa di connessione dall app settings
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string GetConnectionStringByName(string connectionStringName)
        {
            var configuration = AppSetting.GetAppSettingJsonContent();

            var connectionString = configuration.GetConnectionString(connectionStringName);

            if (connectionString == null)
            {
                throw new Exception($"La stringa di connessione \"{connectionStringName}\" non esiste nell' appsettings");
            }
            return connectionString;
        }


        public static string OcStanceConnection
        {
            get
            {
                return GetConnectionStringByName(nameof(ConnectionStrings.OcStanceConnection));
            }
        }

        public static string StocksystemArcoreConnection
        {
            get
            {
                return GetConnectionStringByName(nameof(ConnectionStrings.StocksystemArcoreConnection));
            }
        }

        public static string PimConnection
        {
            get
            {
                return GetConnectionStringByName(nameof(ConnectionStrings.PimConnection));
            }
        }
    }
}
