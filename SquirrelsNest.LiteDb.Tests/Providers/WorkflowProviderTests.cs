using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using LiteDB;
using NSubstitute;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.DatabaseTests.Support;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Providers;
using Xunit;

namespace SquirrelsNest.LiteDb.Tests.Providers {
    [Collection(nameof(SequentialCollection))]
    public class WorkflowProviderTests : IDisposable {
        private readonly IEnvironment           mEnvironment;
        private readonly IApplicationConstants  mConstants;

        private string      TestDirectory => Path.GetTempPath();
        private string      DatabaseFile => Path.Combine( mEnvironment.DatabaseDirectory(), mConstants.DatabaseFileName );

        public WorkflowProviderTests() {
            mEnvironment = Substitute.For<IEnvironment>();
            mEnvironment.DatabaseDirectory().Returns( TestDirectory );

            mConstants = Substitute.For<IApplicationConstants>();
            mConstants.DatabaseFileName.Returns( "Project.DB" );

            DeleteDatabase();
        }

        private WorkflowStateProvider CreateSut() {
            return new WorkflowStateProvider( new DatabaseProvider( mEnvironment, mConstants ));
        }

        [Fact]
        public void StateCanBeStored() {
            using var sut = CreateSut();
            var state = new SnWorkflowState( ObjectId.NewObjectId().ToString(), String.Empty, "project ID", "state name", "Description", StateCategory.Intermediate );

            var result = sut.AddState( state );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a workflow state" ));
            result.IsRight.Should().BeTrue( "workflow states should be addable" );
        }

        [Fact]
        public void NewStateCanBeRetrieved() {
            var state = new SnWorkflowState( "state" ).With( description: "state description", category: StateCategory.Completed );
            using var sut = CreateSut();

            sut.AddState( state );
            var result = sut.GetState( state.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a workflow state" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( state, option => option.Excluding( e => e.DbId ), "retrieved state should match stored state" ));
        }

        [Fact]
        public void StateShouldUpdateSuccessfully() {
            var state = new SnWorkflowState( "my state" );
            using var sut = CreateSut();

            sut.AddState( state ).Do( e => state = e );
            state = state.With( description: "new description" );
            var result = sut.UpdateState( state );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating workflow state" ));
        }

        [Fact]
        public void StateShouldBeUpdated() {
            var state = new SnWorkflowState( "state 1" );
            using var sut = CreateSut();

            sut.AddState( state ).Do( e => state = e );
            state = state.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            var result = sut.UpdateState( state ).Bind( _ => sut.GetState( state.EntityId ));

            result.IfLeft( error => error.Should().BeNull( "updating workflow state should not cause error" ));
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( state, "retrieved state should match update" ));
        }

        [Fact]
        public void StateCanBeDeleted() {
            var state = new SnWorkflowState( "state title" );
            using var sut = CreateSut();

            sut.AddState( state ).Do( e => state = e );
            var result = sut.DeleteState( state );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete workflow state" ));
            result.IsRight.Should().BeTrue( "state should be deleted without error" );
        }

        [Fact]
        public void StatesCanBeListed() {
            var state = new SnWorkflowState( "one" );
            using var sut = CreateSut();

            sut.AddState( state );
            sut.AddState( state.With( name: "two" ));
            sut.AddState( state.With( name: "three" ));
            sut.AddState( state.With( name: "four" ).With( description: "description" ));

            var result = sut.GetStates();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting workflow state list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 workflow states were added" ));
        }

        [Fact]
        public void StatesCanBeListedByProject() {
            var project1 = new SnProject( "project1", "P1" );
            var project2 = new SnProject( "project2", "P2" );
            var state1 = new SnWorkflowState( "state 1" ).For( project1 );
            var state2 = new SnWorkflowState( "state 2" ).For( project1 ).With( description: "description2" );
            var state3 = new SnWorkflowState( "state 3" ).For( project2 ).With( description: "description3" );
            var state4 = new SnWorkflowState( "state 4" ).For( project1 );
            using var sut = CreateSut();

            sut.AddState( state1 );
            sut.AddState( state2 );
            sut.AddState( state3 );
            sut.AddState( state4 );

            var result = sut.GetStates( project1 );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue type list" ));
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 states are associated with project1" ));
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
