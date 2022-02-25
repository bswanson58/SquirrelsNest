using System;
using System.IO;
using FluentAssertions;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using SquirrelsNest.LiteDb.Tests.Database;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class IssueUserTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public IssueUserTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            DeleteDatabase();
        }

        private IssueProvider CreateSut() {
            return new IssueProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        [Fact]
        public void IssueIsCreatedWithDefaultUser() {
            var sut = new SnIssue( "title", 1, EntityId.Default );

            sut.IssueTypeId.Should().BeEquivalentTo( EntityId.Default, "Issue should be created with a default user." );
        }

        [Fact]
        public void StoredIssueReturnsDefaultUser() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.EnteredById.Should().BeEquivalentTo( EntityId.Default, "issue should have default entry user" ));
        }

        [Fact]
        public void IssueStoredWithUserReturnsUser() {
            using var sut = CreateSut();
            var user = new SnUser( "entity id", String.Empty, "userLogin", "user name", "email" );
            var issue = new SnIssue( "title", 3, EntityId.Default ).With( enteredBy: user.EntityId );
            sut.AddIssue( issue ).Do( i => issue = i );

            var results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.Should().BeEquivalentTo( issue, "issue should have updated user" ));
            results.IfRight( retrieved => retrieved.EnteredById.Should().BeEquivalentTo( user.EntityId, "issue type should be equal to stored" ));
        }

        [Fact]
        public void StoredIssueReturnsDefaultAssignedUser() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.AssignedToId.Should().BeEquivalentTo( EntityId.Default, "issue should have default assigned user" ));
        }

        [Fact]
        public void IssueStoredWithAssignedReturnsAssigned() {
            using var sut = CreateSut();
            var user = new SnUser( "entity id", String.Empty, "userLogin", "user name", "email" );
            var issue = new SnIssue( "title", 3, EntityId.Default ).With( assignedTo: user.EntityId );
            sut.AddIssue( issue ).Do( i => issue = i );

            var results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.Should().BeEquivalentTo( issue, "issue should have updated assigned user" ));
            results.IfRight( retrieved => retrieved.AssignedToId.Should().BeEquivalentTo( user.EntityId, "issue type should be equal to stored" ));
        }

        private void DeleteDatabase() {
            if( File.Exists( DatabaseFile )) {
                File.Delete( DatabaseFile );
            }
        }

        public void Dispose() {
            DeleteDatabase();
        }
    }
}
