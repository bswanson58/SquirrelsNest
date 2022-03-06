using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.DatabaseTests.Providers;
using SquirrelsNest.DatabaseTests.Support;
using SquirrelsNest.EfDb.Providers;
using Xunit;

namespace SquirrelsNest.EfDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class ReleaseProviderTests : ReleaseProviderTestSuite {
        protected override IReleaseProvider CreateSut() {
            return new ReleaseProvider( new TestContextFactory());
        }

        protected override void DeleteDatabase() {
            var factory = new TestContextFactory();

            factory.ProvideContext().Database.EnsureDeleted();
        }
    }
}