using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class ReleaseTypeProvider : BaseProviderTests {
        private IReleaseProvider CreateSut() {
            return new ReleaseProvider( mIssueProvider, mReleaseProvider );
        }

        [Fact]
        public async void DeletedReleaseAffectsIssues() {
            var releases = await CreateSomeReleases( 3 );
            var issues = await CreateSomeIssues( 3, releases );
            using var sut = CreateSut();

            var deleteResult = await sut.DeleteRelease( releases[1] );
            var getIssue1Result = await mIssueProvider.GetIssue( issues[1].EntityId );
            var listResult = await sut.GetReleases( SnProject.Default );

            deleteResult.IfLeft( error => error.Should().BeNull( "should be no error deleting a release" ));
            getIssue1Result.IfLeft( error => error.Should().BeNull( "error retrieving issue 1" ));
            getIssue1Result.IfRight( issue => issue.ReleaseId.Should().BeEquivalentTo( EntityId.Default, "release should be removed" ));
            listResult.IfRight( list => list.Length().Should().Be( 2, "one release should be deleted" ));
        }
    }
}
