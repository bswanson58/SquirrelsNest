using System;
using System.Linq;
using FluentAssertions;
using LanguageExt;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class IssueProviderTestSuite : BaseProviderTestSuite {
        private readonly DateTime   mTestTime;

        protected IssueProviderTestSuite() {
            mTestTime = new DateTime( 2020, 12, 17, 6, 33, 44 );

            DateTimeProvider.SetProvider( new TestTimeProvider( mTestTime ));
        }

        protected abstract IDbIssueProvider CreateSut();

        [Fact]
        public async void IssueCanBeStored() {
            using var sut = CreateSut();
            var issue = new SnIssue( EntityId.Default, String.Empty, "Title", "Description", "ProjectID", 2, DateTimeProvider.Instance.CurrentDate, 
                                     EntityId.Default, EntityId.Default, EntityId.Default, EntityId.Default, EntityId.Default, EntityId.Default );

            var result = await sut.AddIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding an issue" ));
            result.IsRight.Should().BeTrue( "issues should be addable" );
        }

        [Fact]
        public async void NewIssueCanBeRetrieved() {
            var issue = new SnIssue( "title", 1, EntityId.Default );
            using var sut = CreateSut();

            await sut.AddIssue( issue );
            var result = await sut.GetIssue( issue.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving an issue" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( issue, option => option.Excluding( e => e.DbId ), "retrieved issue should match stored issue" ));
        }

        [Fact]
        public async void NewIssueShouldHaveCurrentEntryDate() {
            var issue = new SnIssue( "title", 2, EntityId.Default );
            using var sut = CreateSut();

            await sut.AddIssue( issue );
            var result = await sut.GetIssue( issue.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving an issue" ));
            result.Do( retrieved => retrieved.EntryDate.Should().Be( DateOnly.FromDateTime( mTestTime ), "entry date should match test time" ));
        }

        [Fact]
        public async void IssueShouldUpdateSuccessfully() {
            var issue = new SnIssue( "title", 1, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Result.Do( e => issue = e );
            issue = issue.With( description: "new description" );
            var result = await sut.UpdateIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating issue" ));
        }

        [Fact]
        public async void IssueShouldBeUpdated() {
            var issue = new SnIssue( "title", 3, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Result.Do( e => issue = e );
            issue = issue.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = await sut.UpdateIssue( issue ).Bind( _ => sut.GetIssue( issue.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating issue should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( issue, "retrieved issue should match update" ));
        }

        [Fact]
        public async void IssueCanBeDeleted() {
            var issue = new SnIssue( "issue title", 100, EntityId.Default );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Result.Do( e => issue = e );
            var result = await sut.DeleteIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete entity" ));
            result.IsRight.Should().BeTrue( "entity should be deleted without error" );
        }

        [Fact]
        public async void IssuesCanBeListed() {
            var issue = new SnIssue( "one", 1, EntityId.Default );
            using var sut = CreateSut();

            await sut.AddIssue( issue );
            await sut.AddIssue( issue.With( title: "two" ));
            await sut.AddIssue( issue.With( title: "three" ));
            await sut.AddIssue( issue.With( title: "four" ));

            var result = await sut.GetIssues();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 issues were added" ));
        }
    }
}
