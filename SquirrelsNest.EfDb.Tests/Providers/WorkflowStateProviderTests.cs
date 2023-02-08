using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.DatabaseTests.Providers;
using SquirrelsNest.DatabaseTests.Support;
using SquirrelsNest.EfDb.Providers;
using Xunit;

namespace SquirrelsNest.EfDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class WorkflowStateProviderTests : WorkflowStateProviderTestSuite {
        protected override IDbWorkflowStateProvider CreateSut() {
            return new WorkflowStateProvider( new TestContextFactory());
        }

        protected override void DeleteDatabase() {
            var factory = new TestContextFactory();

            factory.ProvideContext().Database.EnsureDeleted();
        }
    }
}