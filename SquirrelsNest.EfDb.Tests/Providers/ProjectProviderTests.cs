﻿using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.DatabaseTests.Providers;
using SquirrelsNest.DatabaseTests.Support;
using SquirrelsNest.EfDb.Providers;
using Xunit;

namespace SquirrelsNest.EfDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class ProjectProviderTests : ProjectProviderTestSuite {
        protected override IProjectProvider CreateSut() {
            return new ProjectProvider( new TestContextFactory());
        }

        protected override void DeleteDatabase() {
            var factory = new TestContextFactory();

            factory.ProvideContext().Database.EnsureDeleted();
        }
    }
}