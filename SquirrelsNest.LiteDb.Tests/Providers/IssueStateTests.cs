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
    public class IssueStateTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;
        private readonly List<SnWorkflowState>  mWorkflowStates;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public IssueStateTests() {
            DateTimeProvider.SetProvider( new TestTimeProvider( new DateTime( 2020, 11, 30, 7, 17, 55 )));

            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            mWorkflowStates = new List<SnWorkflowState>();

            DeleteDatabase();
        }

        private IssueProvider CreateSut() {
            AddSomeStates();

            return new IssueProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        private void AddSomeStates() {
            using var releaseProvider = new WorkflowStateProvider( new DatabaseProvider( mEnvironment, mConstants ));

            releaseProvider.AddState( new SnWorkflowState( "state 1" )).Do( release => mWorkflowStates.Add( release ));
            releaseProvider.AddState( new SnWorkflowState( "state 2" )).Do( release => mWorkflowStates.Add( release ));
            releaseProvider.AddState( new SnWorkflowState( "state 3" )).Do( release => mWorkflowStates.Add( release ));
        }

        [Fact]
        public void IssueIsCreatedWithDefaultState() {
            var sut = new SnIssue( "title", 1, EntityId.Default );

            sut.WorkflowStateId.Should().BeEquivalentTo( EntityId.Default, "Issue should be created with a default state." );
        }

        [Fact]
        public void StoredIssueWithoutStateReturnsSame() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.WorkflowStateId.Should().BeEquivalentTo( EntityId.Default, "issue should have default state" ));
        }

        [Fact]
        public void IssueStoredWithStateReturnsState() {
            using var sut = CreateSut();
            var issue = new SnIssue( "title", 3, EntityId.Default );
            sut.AddIssue( issue );

            var results = sut.GetIssue( issue.EntityId );
            results.IfRight( i => issue = i.With( mWorkflowStates[1]));
            sut.UpdateIssue( issue );
            results = sut.GetIssue( issue.EntityId );

            results.IfLeft( error => error.Should().BeNull( "issue should be retrievable" ));
            results.IfRight( retrieved => retrieved.Should().BeEquivalentTo( issue, "issue should have updated state" ));
            results.IfRight( retrieved => retrieved.WorkflowStateId.Should().BeEquivalentTo( mWorkflowStates[1].EntityId, "issue state should be equal to stored" ));
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
