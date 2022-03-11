using FluentAssertions;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class UserProviderTests : BaseProviderTests {
        private IUserProvider CreateSut() {
            return new UserProvider( mIssueProvider, mUserProvider );
        }

        [Fact]
        public async void DeletedAssignedUserAffectsIssues() {
            var assignedBy = await CreateSomeUsers( 3 );
            var enteredBy = await CreateSomeUsers( 3 );
            var issues = await CreateSomeIssues( 3, enteredBy, assignedBy );
            using var sut = CreateSut();

            var deleteResult = await sut.DeleteUser( assignedBy[1] );
            var getIssue1Result = await mIssueProvider.GetIssue( issues[1].EntityId );
            var listResult = await sut.GetUsers();

            deleteResult.IfLeft( error => error.Should().BeNull( "should be no error deleting a user" ));
            getIssue1Result.IfLeft( error => error.Should().BeNull( "error retrieving issue 1" ));
            getIssue1Result.IfRight( issue => issue.AssignedToId.Should().BeEquivalentTo( EntityId.Default, "user should be removed" ));
            listResult.IfRight( list => list.Length().Should().Be( 5, "one user should be deleted" ));
        }

        [Fact]
        public async void DeletedEntryUserAffectsIssues() {
            var assignedBy = await CreateSomeUsers( 3 );
            var enteredBy = await CreateSomeUsers( 3 );
            var issues = await CreateSomeIssues( 3, enteredBy, assignedBy );
            using var sut = CreateSut();

            var deleteResult = await sut.DeleteUser( enteredBy[1] );
            var getIssue1Result = await mIssueProvider.GetIssue( issues[1].EntityId );
            var listResult = await sut.GetUsers();

            deleteResult.IfLeft( error => error.Should().BeNull( "should be no error deleting a user" ));
            getIssue1Result.IfLeft( error => error.Should().BeNull( "error retrieving issue 1" ));
            getIssue1Result.IfRight( issue => issue.EnteredById.Should().BeEquivalentTo( EntityId.Default, "user should be removed" ));
            listResult.IfRight( list => list.Length().Should().Be( 5, "one user should be deleted" ));
        }
    }
}
