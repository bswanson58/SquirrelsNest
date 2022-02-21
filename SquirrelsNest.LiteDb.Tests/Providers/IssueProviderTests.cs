using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using LiteDB;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using SquirrelsNest.LiteDb.Tests.Database;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class IssueProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly DateTime               mTestTime;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public IssueProviderTests() {
            mTestTime = new DateTime( 2020, 12, 17, 6, 33, 44 );

            DateTimeProvider.SetProvider( new TestTimeProvider( mTestTime ));

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
        public void IssueCanBeStored() {
            using var sut = CreateSut();
            var issue = new SnIssue( EntityId.Default, ObjectId.NewObjectId().ToString(), "Title", "Description", "ProjectID", 2, DateTimeProvider.Instance.CurrentDate, 
                                     EntityId.Default, EntityId.Default, EntityId.Default, EntityId.Default, EntityId.Default, EntityId.Default );

            var result = sut.AddIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding an issue" ));
            result.IsRight.Should().BeTrue( "issues should be addable" );
        }

        [Fact]
        public void NewIssueCanBeRetrieved() {
            var issue = new SnIssue( "title", 1, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue );
            var result = sut.GetIssue( issue.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving an issue" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( issue, option => option.Excluding( e => e.DbId ), "retrieved issue should match stored issue" ));
        }

        [Fact]
        public void NewIssueShouldHaveCurrentEntryDate() {
            var issue = new SnIssue( "title", 2, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue );
            var result = sut.GetIssue( issue.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving an issue" ));
            result.Do( retrieved => retrieved.EntryDate.Should().Be( DateOnly.FromDateTime( mTestTime ), "entry date should match test time" ));
        }

        [Fact]
        public void IssueShouldUpdateSuccessfully() {
            var issue = new SnIssue( "title", 1, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Do( e => issue = e );
            issue = issue.With( description: "new description" );
            var result = sut.UpdateIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating issue" ));
        }

        [Fact]
        public void IssueShouldBeUpdated() {
            var issue = new SnIssue( "title", 3, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Do( e => issue = e );
            issue = issue.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = sut.UpdateIssue( issue ).Bind( _ => sut.GetIssue( issue.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating issue should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( issue, "retrieved issue should match update" ));
        }

        [Fact]
        public void IssueCanBeDeleted() {
            var issue = new SnIssue( "issue title", 100, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Do( e => issue = e );
            var result = sut.DeleteIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete entity" ));
            result.IsRight.Should().BeTrue( "entity should be deleted without error" );
        }

        [Fact]
        public void IssuesCanBeListed() {
            var issue = new SnIssue( "one", 1, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue );
            sut.AddIssue( issue.With( title: "two" ));
            sut.AddIssue( issue.With( title: "three" ));
            sut.AddIssue( issue.With( title: "four" ));

            var result = sut.GetIssues();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 issues were added" ));
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
