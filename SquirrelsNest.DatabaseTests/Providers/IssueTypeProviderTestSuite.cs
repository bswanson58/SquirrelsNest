using System;
using System.Linq;
using FluentAssertions;
using LanguageExt;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class IssueTypeProviderTestSuite : BaseProviderTestSuite {
        protected abstract IDbIssueTypeProvider CreateSut();

        [Fact]
        public async void IssueTypeCanBeStored() {
            using var sut = CreateSut();
            var issue = new SnIssueType( EntityId.Default, String.Empty, "project ID", "Name", "Description" );

            var result = await sut.AddIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding an issue type" ));
            result.IsRight.Should().BeTrue( "issue types should be addable" );
        }

        [Fact]
        public async void NewIssueTypeCanBeRetrieved() {
            var issue = new SnIssueType( "Feature" ).With( description: "description" );
            using var sut = CreateSut();

            await sut.AddIssue( issue );
            var result = await sut.GetIssue( issue.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving an issue type" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( issue, option => option.Excluding( e => e.DbId ), "retrieved issue should match stored issue" ));
        }

        [Fact]
        public async void IssueTypeShouldUpdateSuccessfully() {
            var issue = new SnIssueType( "bug" );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Result.Do( e => issue = e );
            issue = issue.With( description: "new description" );
            var result = await  sut.UpdateIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating issue type" ));
        }

        [Fact]
        public async void IssueTypeShouldBeUpdated() {
            var issue = new SnIssueType( "feature" );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Result.Do( e => issue = e );
            issue = issue.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = await sut.UpdateIssue( issue ).Bind( _ => sut.GetIssue( issue.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating issue type should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( issue, "retrieved issue type should match update" ));
        }

        [Fact]
        public async void IssueTypeCanBeDeleted() {
            var issue = new SnIssueType( "bug-a-boo" );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Result.Do( e => issue = e );
            var result = await sut.DeleteIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete issue type" ));
            result.IsRight.Should().BeTrue( "issue type should be deleted without error" );
        }

        [Fact]
        public async void IssueTypesCanBeListed() {
            var issue = new SnIssueType( "one" );
            using var sut = CreateSut();

            await sut.AddIssue( issue );
            await sut.AddIssue( issue.With( name: "two" ));
            await sut.AddIssue( issue.With( name: "three" ));
            await sut.AddIssue( issue.With( name: "four" ).With( description: "description" ));

            var result = await sut.GetIssues();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue type list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 issue types were added" ));
        }

        [Fact]
        public async void IssueTypesCanBeListedByProject() {
            var project1 = new SnProject( "project1", "P1" );
            var project2 = new SnProject( "project2", "P1" );
            var issue1 = new SnIssueType( "issue1" ).For( project1 );
            var issue2 = new SnIssueType( "issue2" ).For( project1 ).With( description: "description2" );
            var issue3 = new SnIssueType( "issue3" ).For( project2 ).With( description: "description3" );
            var issue4 = new SnIssueType( "issue4" ).For( project1 );
            using var sut = CreateSut();

            await sut.AddIssue( issue1 );
            await sut.AddIssue( issue2 );
            await sut.AddIssue( issue3 );
            await sut.AddIssue( issue4 );

            var result = await sut.GetIssues( project1 );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue type list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 issue types are associated with project1" ));
        }
    }
}
