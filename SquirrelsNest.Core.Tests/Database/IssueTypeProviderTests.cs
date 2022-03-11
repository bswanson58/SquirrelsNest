using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class IssueTypeProviderTests : BaseProviderTests {
        private IIssueTypeProvider CreateSut() {
            return new IssueTypeProvider( mIssueProvider, mIssueTypeProvider );
        }

        [Fact]
        public async void DeletedIssueTypeAffectsIssues() {
            var issueTypes = await CreateSomeIssueTypes( 3 );
            var issues = await CreateSomeIssues( 3, issueTypes );
            using var sut = CreateSut();

            var deleteResult = await sut.DeleteIssue( issueTypes[1] );
            var getIssue1Result = await mIssueProvider.GetIssue( issues[1].EntityId );
            var listResult = await sut.GetIssues( SnProject.Default );

            deleteResult.IfLeft( error => error.Should().BeNull( "should be no error deleting an issue type" ));
            getIssue1Result.IfLeft( error => error.Should().BeNull( "error retrieving issue 1" ));
            getIssue1Result.IfRight( issue => issue.IssueTypeId.Should().BeEquivalentTo( EntityId.Default, "issue type should be removed" ));
            listResult.IfRight( list => list.Length().Should().Be( 2, "one issue type should be deleted" ));
        }
    }
}
