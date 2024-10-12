using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HelpMi.EL.HelpMiDb
{
    public class HelpMiDbContextFactory : IDesignTimeDbContextFactory<HelpMiDbContext>
    {
        public HelpMiDbContext CreateDbContext(string[] args)
        {
            // Recupera la configurazione dal file appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Configura DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<HelpMiDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new HelpMiDbContext(optionsBuilder.Options);
        }
    }
}
