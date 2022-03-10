using System;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class ComponentProviderTests : IDisposable {
        private readonly IDbIssueProvider       mIssueProvider;
        private readonly IDbComponentProvider   mComponentProvider;

        public ComponentProviderTests() {
            var contextFactory = new EfContextFactory();

            mIssueProvider = new EfDb.Providers.IssueProvider( contextFactory );
            mComponentProvider = new EfDb.Providers.ComponentProvider( contextFactory );
        }

        private IComponentProvider CreateSut() {
            return new ComponentProvider( mIssueProvider, mComponentProvider );
        }

        [Fact]
        public async void DeletedComponentAffectsIssues() {
            var component1 = new SnComponent( "Component 1" ).For( SnProject.Default );
            var component2 = new SnComponent( "Component 2" ).For( SnProject.Default );
            var issue1 = new SnIssue( "Issue 1", 1, EntityId.Default );
            var issue2 = new SnIssue( "Issue 2", 2, EntityId.Default );
            using var sut = CreateSut();

            mComponentProvider.AddComponent( component1 ).Result.Do( c => component1 = c );
            mComponentProvider.AddComponent( component2 ).Result.Do( c => component2 = c );
            mIssueProvider.AddIssue( issue1.With( component1 )).Result.Do( i => issue1 = i );
            mIssueProvider.AddIssue( issue2.With( component2 )).Result.Do( i => issue2 = i );
            var deleteResult = await sut.DeleteComponent( component1 );
            var getIssue1Result = await mIssueProvider.GetIssue( issue1.EntityId );
            var getIssue2Result = await mIssueProvider.GetIssue( issue2.EntityId );
            var componentsResult = await mComponentProvider.GetComponents( SnProject.Default );

            deleteResult.IfLeft( error => error.Should().BeNull( "should be no error deleting a component" ));
            getIssue1Result.IfLeft( error => error.Should().BeNull( "error retrieving issue 1" ));
            getIssue2Result.IfLeft( error => error.Should().BeNull( "error retrieving issue 2" ));
            getIssue1Result.IfRight( issue => issue.ComponentId.Should().BeEquivalentTo( EntityId.Default, "component should be removed" ));
            componentsResult.IfRight( components => components.Length().Should().Be( 1, "component should be deleted" ));
        }

        private void DeleteDatabase() {
            var factory = new EfContextFactory();

            factory.ProvideContext().Database.EnsureDeleted();
        }

        public void Dispose() {
            mIssueProvider.Dispose();
            mComponentProvider.Dispose();

            DeleteDatabase();
        }
    }
}
