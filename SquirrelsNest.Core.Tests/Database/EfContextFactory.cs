using Microsoft.EntityFrameworkCore;
using SquirrelsNest.EfDb;
using SquirrelsNest.EfDb.Context;

namespace SquirrelsNest.Core.Tests.Database {
    internal class EfContextFactory : IContextFactory {
        public SquirrelsNestDbContext ProvideContext() {
            var options = new DbContextOptionsBuilder<SquirrelsNestDbContext>()
                .UseInMemoryDatabase( "TestDB" );
//                .UseSqlServer( "Server=(localdb)\\MSSQLLocalDB;Database=SquirrelsNestDB;Trusted_Connection=True;MultipleActiveResultSets=true" );

            return new SquirrelsNestDbContext( options.Options );
        }
    }
}
