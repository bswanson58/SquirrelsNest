using System;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.DatabaseTests.Providers;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Providers;

namespace SquirrelsNest.EfDb.Tests.Providers {
    internal class TestContextFactory : IContextFactory {
        public SquirrelsNestDbContext ProvideContext() {
            var options = new DbContextOptionsBuilder<SquirrelsNestDbContext>()
                .UseInMemoryDatabase( "TestDB" );

            return new SquirrelsNestDbContext( options.Options );
        }
    }

    public class ComponentProviderTests : ComponentProviderTestSuite, IDisposable {
        protected override IComponentProvider CreateSut() {
            return new ComponentProvider( new TestContextFactory());
        }

        protected override void DeleteDatabase() {
            var factory = new TestContextFactory();

            factory.ProvideContext().Database.EnsureDeleted();
        }
    }
}
