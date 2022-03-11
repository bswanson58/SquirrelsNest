using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class ComponentProviderTests : BaseProviderTests {

        private IComponentProvider CreateSut() {
            return new ComponentProvider( mIssueProvider, mComponentProvider );
        }

        [Fact]
        public async void DeletedComponentAffectsIssues() {
            var components = await CreateSomeComponents( 3 );
            var issues = await CreateSomeIssues( 3, components );
            using var sut = CreateSut();

            var deleteResult = await sut.DeleteComponent( components[0] );
            var getIssue1Result = await mIssueProvider.GetIssue( issues[0].EntityId );
            var componentsResult = await mComponentProvider.GetComponents( SnProject.Default );

            deleteResult.IfLeft( error => error.Should().BeNull( "should be no error deleting a component" ));
            getIssue1Result.IfLeft( error => error.Should().BeNull( "error retrieving issue 1" ));
            getIssue1Result.IfRight( issue => issue.ComponentId.Should().BeEquivalentTo( EntityId.Default, "component should be removed" ));
            componentsResult.IfRight( componentList => componentList.Length().Should().Be( 2, "component should be deleted" ));
        }
    }
}
