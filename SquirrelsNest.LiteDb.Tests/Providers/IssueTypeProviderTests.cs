using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using LiteDB;
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
    public class IssueTypeProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public IssueTypeProviderTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            DeleteDatabase();
        }

        private IssueTypeProvider CreateSut() {
            return new IssueTypeProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        [Fact]
        public void IssueTypeCanBeStored() {
            using var sut = CreateSut();
            var issue = new SnIssueType( EntityId.Default, ObjectId.NewObjectId().ToString(), "project ID", "Name", "Description" );

            var result = sut.AddIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding an issue type" ));
            result.IsRight.Should().BeTrue( "issue types should be addable" );
        }

        [Fact]
        public void NewIssueTypeCanBeRetrieved() {
            var issue = new SnIssueType( "Feature" ).With( description: "description" );
            using var sut = CreateSut();

            sut.AddIssue( issue );
            var result = sut.GetIssue( issue.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving an issue type" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( issue, option => option.Excluding( e => e.DbId ), "retrieved issue should match stored issue" ));
        }

        [Fact]
        public void IssueTypeShouldUpdateSuccessfully() {
            var issue = new SnIssueType( "bug" );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Do( e => issue = e );
            issue = issue.With( description: "new description" );
            var result = sut.UpdateIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating issue type" ));
        }

        [Fact]
        public void IssueTypeShouldBeUpdated() {
            var issue = new SnIssueType( "feature" );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Do( e => issue = e );
            issue = issue.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = sut.UpdateIssue( issue ).Bind( _ => sut.GetIssue( issue.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating issue type should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( issue, "retrieved issue type should match update" ));
        }

        [Fact]
        public void IssueTypeCanBeDeleted() {
            var issue = new SnIssueType( "bug-a-boo" );
            using var sut = CreateSut();

            sut.AddIssue( issue ).Do( e => issue = e );
            var result = sut.DeleteIssue( issue );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete issue type" ));
            result.IsRight.Should().BeTrue( "issue type should be deleted without error" );
        }

        [Fact]
        public void IssueTypesCanBeListed() {
            var issue = new SnIssueType( "one" );
            using var sut = CreateSut();

            sut.AddIssue( issue );
            sut.AddIssue( issue.With( name: "two" ));
            sut.AddIssue( issue.With( name: "three" ));
            sut.AddIssue( issue.With( name: "four" ).With( description: "description" ));

            var result = sut.GetIssues();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue type list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 issue types were added" ));
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
