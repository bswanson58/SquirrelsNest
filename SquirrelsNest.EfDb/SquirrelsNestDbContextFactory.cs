using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SquirrelsNest.EfDb {
    // For EF Migrations
    internal class SquirrelsNestDbContextFactory : IDesignTimeDbContextFactory<SquirrelsNestDbContext> {
        public SquirrelsNestDbContext CreateDbContext( string[] args ) {
            var optionsBuilder = new DbContextOptionsBuilder<SquirrelsNestDbContext>();
            optionsBuilder.UseSqlServer( "Server=(localdb)\\MSSQLLocalDB;Database=SquirrelsNestDB;Trusted_Connection=True;MultipleActiveResultSets=true" );

            return new SquirrelsNestDbContext( optionsBuilder.Options );
        }
    }
}
