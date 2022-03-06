using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.EfDb.Context {
    internal class ConfigurationBuilder {
        private readonly IPreferences<EfDatabaseConfiguration>  mConfiguration;

        public ConfigurationBuilder( IPreferences<EfDatabaseConfiguration> configuration ) {
            mConfiguration = configuration;
        }

        public DbContextOptions Options() {
            var configuration = mConfiguration.Current;

            if( String.IsNullOrWhiteSpace( configuration.ConnectionString )) {
                throw new ApplicationException( "SQL Server connection string must be configured in EfDatabaseConfiguration" );
            }

            var options = new DbContextOptionsBuilder<SquirrelsNestDbContext>()
                .UseSqlServer( configuration.ConnectionString );

            return options.Options;
        }
    }
}
