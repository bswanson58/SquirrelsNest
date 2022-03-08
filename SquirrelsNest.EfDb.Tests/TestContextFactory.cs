using Microsoft.EntityFrameworkCore;
using SquirrelsNest.EfDb.Context;

namespace SquirrelsNest.EfDb.Tests {
    internal class TestContextFactory : IContextFactory {
        public SquirrelsNestDbContext ProvideContext() {
            var options = new DbContextOptionsBuilder<SquirrelsNestDbContext>()
                .UseInMemoryDatabase( "TestDB" );
//                .UseSqlServer( "Server=(localdb)\\MSSQLLocalDB;Database=SquirrelsNestDB;Trusted_Connection=True;MultipleActiveResultSets=true" );

            return new SquirrelsNestDbContext( options.Options );
        }
    }
}
