using System;
using System.Linq;
using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using Xunit;

namespace SquirrelsNest.DatabaseTests.Providers {
    public abstract class WorkflowStateProviderTestSuite : BaseProviderTestSuite {
        protected abstract IWorkflowStateProvider CreateSut();

        [Fact]
        public async void StateCanBeStored() {
            using var sut = CreateSut();
            var state = new SnWorkflowState( EntityId.Default, String.Empty, "project ID", "state name", "Description", StateCategory.Intermediate );

            var result = await sut.AddState( state );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred adding a workflow state" ) );
            result.IsRight.Should().BeTrue( "workflow states should be addable" );
        }

        [Fact]
        public async void NewStateCanBeRetrieved() {
            var state = new SnWorkflowState( "state" ).With( description: "state description", category: StateCategory.Completed );
            using var sut = CreateSut();

            await sut.AddState( state );
            var result = await sut.GetState( state.EntityId );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred retrieving a workflow state" ) );
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( state, option => option.Excluding( e => e.DbId ), "retrieved state should match stored state" ) );
        }

        [Fact]
        public async void StateShouldUpdateSuccessfully() {
            var state = new SnWorkflowState( "my state" );
            using var sut = CreateSut();

            sut.AddState( state ).Result.Do( e => state = e );
            state = state.With( description: "new description" );
            var result = await sut.UpdateState( state );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred updating workflow state" ) );
        }

        [Fact]
        public async void StateShouldBeUpdated() {
            var state = new SnWorkflowState( "state 1" );
            using var sut = CreateSut();

            sut.AddState( state ).Result.Do( e => state = e );
            state = state.With( description: "new description" );
            // ReSharper disable once AccessToDisposedClosure
            await sut.UpdateState( state );
            var result = await sut.GetState( state.EntityId );

            result.IfLeft( error => error.Should().BeNull( "updating workflow state should not cause error" ) );
            result.Do( retrieved => retrieved.Should().BeEquivalentTo( state, "retrieved state should match update" ) );
        }

        [Fact]
        public async void StateCanBeDeleted() {
            var state = new SnWorkflowState( "state title" );
            using var sut = CreateSut();

            sut.AddState( state ).Result.Do( e => state = e );
            var result = await sut.DeleteState( state );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} during delete workflow state" ) );
            result.IsRight.Should().BeTrue( "state should be deleted without error" );
        }

        [Fact]
        public async void StatesCanBeListed() {
            var state = new SnWorkflowState( "one" );
            using var sut = CreateSut();

            await sut.AddState( state );
            await sut.AddState( state.With( name: "two" ) );
            await sut.AddState( state.With( name: "three" ) );
            await sut.AddState( state.With( name: "four" ).With( description: "description" ) );

            var result = await sut.GetStates();

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting workflow state list" ) );
            result.IfRight( enumerator => enumerator.Count().Should().Be( 4, "4 workflow states were added" ) );
        }

        [Fact]
        public async void StatesCanBeListedByProject() {
            var project1 = new SnProject( "project1", "P1" );
            var project2 = new SnProject( "project2", "P2" );
            var state1 = new SnWorkflowState( "state 1" ).For( project1 );
            var state2 = new SnWorkflowState( "state 2" ).For( project1 ).With( description: "description2" );
            var state3 = new SnWorkflowState( "state 3" ).For( project2 ).With( description: "description3" );
            var state4 = new SnWorkflowState( "state 4" ).For( project1 );
            using var sut = CreateSut();

            await sut.AddState( state1 );
            await sut.AddState( state2 );
            await sut.AddState( state3 );
            await sut.AddState( state4 );

            var result = await sut.GetStates( project1 );

            result.IfLeft( error => error.Should().BeNull( $"{error.Message} occurred while getting issue type list" ) );
            result.IfRight( enumerator => enumerator.Count().Should().Be( 3, "3 states are associated with project1" ) );
        }
    }

}