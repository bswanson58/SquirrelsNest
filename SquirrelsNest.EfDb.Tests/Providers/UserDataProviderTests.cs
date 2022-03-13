using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.DatabaseTests.Providers;
using SquirrelsNest.DatabaseTests.Support;
using SquirrelsNest.EfDb.Providers;
using Xunit;

namespace SquirrelsNest.EfDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class UserDataProviderTests : UserDataProviderTestSuite {
        protected override IDbUserDataProvider CreateSut() {
            return new UserDataProvider( new TestContextFactory());
        }

        protected override void DeleteDatabase() {
            var factory = new TestContextFactory();

            factory.ProvideContext().Database.EnsureDeleted();
        }
    }
}
