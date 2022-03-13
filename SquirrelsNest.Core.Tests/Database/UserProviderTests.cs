using System;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class UserProviderTests : BaseProviderTests {
        private IUserProvider CreateSut() {
            return new UserProvider( mIssueProvider, mUserProvider, mUserDataProvider, mAssociationProvider );
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

        [Fact]
        public async void DeletingUserAlsoDeletesUserData() {
            var users = await CreateSomeUsers( 2 );
            var myUser = users[0];
            using var sut = CreateSut();

            await mUserDataProvider.AddData( new SnUserData( myUser.EntityId, UserDataType.LastProject, "last project" ));
            await mUserDataProvider.AddData( new SnUserData( myUser.EntityId, UserDataType.IssueListFormat, "list format" ));
            await sut.DeleteUser( myUser );
            var recreatedUser = new SnUser( myUser.EntityId, String.Empty, myUser.LoginName, myUser.Name, myUser.Email );
            var results = await mUserDataProvider.GetData( recreatedUser );

            results.IfLeft( error => error.Should().BeNull( "error should not be returned getting data for deleted user" ));
            results.IfRight( list => list.Length().Should().Be( 0, "all user data should be deleted" ));
        }
    }
}
