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
    public class IssueReleaseTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly List<SnRelease>        mReleases;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public IssueReleaseTests() {
            DateTimeProvider.SetProvider( new TestTimeProvider( new DateTime( 2020, 11, 30, 7, 17, 55 )));

            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            mReleases = new List<SnRelease>();

            DeleteDatabase();
        }

        private IssueProvider CreateSut() {
            AddSomeReleases();

            return new IssueProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        private void AddSomeReleases() {
            using var releaseProvider = new ReleaseProvider( new DatabaseProvider( mEnvironment, mConstants ));

            releaseProvider.AddRelease( new SnRelease( "release 1" )).Do( release => mReleases.Add( release ));
            releaseProvider.AddRelease( new SnRelease( "release 2" )).Do( release => mReleases.Add( release ));
            releaseProvider.AddRelease( new SnRelease( "release 3" )).Do( release => mReleases.Add( release ));
        }

        [Fact]
        public void IssueIsCreatedWithDefaultRelease() {
            var sut = new SnIssue( "title", 1, EntityId.Default );

            sut.ReleaseId.Should().BeEquivalentTo( EntityId.Default, "Issue should be created with a default release." );
        }

        [Fact]
        public void StoredUnreleasedIssueReturnsUnreleased() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.ReleaseId.Should().BeEquivalentTo( EntityId.Default, "issue should have default release" ));
        }

        [Fact]
        public void IssueStoredWithReleaseReturnsRelease() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );
            results.IfRight( i => issue = i.With( mReleases[1]));
            sut.UpdateIssue( issue );
            results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.Should().BeEquivalentTo( issue, "issue should have updated release" ));
            results.IfRight( retrieved => retrieved.ReleaseId.Should().BeEquivalentTo( mReleases[1].EntityId, "issue release should be equal to stored" ));
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
