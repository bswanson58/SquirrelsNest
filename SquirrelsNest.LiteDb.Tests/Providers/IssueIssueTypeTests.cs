using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;
using SquirrelsNest.DatabaseTests.Support;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class IssueIssueTypeTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly List<SnIssueType>      mIssueTypes;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public IssueIssueTypeTests() {
            DateTimeProvider.SetProvider( new TestTimeProvider( new DateTime( 2020, 11, 30, 7, 17, 55 )));

            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            mIssueTypes = new List<SnIssueType>();

            DeleteDatabase();
        }

        private IssueProvider CreateSut() {
            AddSomeIssueType();

            return new IssueProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        private void AddSomeIssueType() {
            using var releaseProvider = new IssueTypeProvider( new DatabaseProvider( mEnvironment, mConstants ));

            releaseProvider.AddIssue( new SnIssueType( "type 1" )).Do( release => mIssueTypes.Add( release ));
            releaseProvider.AddIssue( new SnIssueType( "type 2" )).Do( release => mIssueTypes.Add( release ));
            releaseProvider.AddIssue( new SnIssueType( "type 3" )).Do( release => mIssueTypes.Add( release ));
        }

        [Fact]
        public void IssueIsCreatedWithDefaultIssueType() {
            var sut = new SnIssue( "title", 1, EntityId.Default );

            sut.IssueTypeId.Should().BeEquivalentTo( EntityId.Default, "Issue should be created with a default issue type." );
        }

        [Fact]
        public void StoredUntypedIssueReturnsUntyped() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.IssueTypeId.Should().BeEquivalentTo( EntityId.Default, "issue should have default issue type" ));
        }

        [Fact]
        public void IssueStoredWithTypeReturnsType() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );
            results.IfRight( i => issue = i.With( mIssueTypes[1]));
            sut.UpdateIssue( issue );
            results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.Should().BeEquivalentTo( issue, "issue should have updated issue type" ));
            results.IfRight( retrieved => retrieved.IssueTypeId.Should().BeEquivalentTo( mIssueTypes[1].EntityId, "issue type should be equal to stored" ));
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
