using FluentAssertions;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Database;
using SquirrelsNest.DatabaseTests.Support;
using Xunit;

namespace SquirrelsNest.Core.Tests.Database {
    [Collection(nameof(SequentialCollection))]
    public class WorkflowStateProviderTests : BaseProviderTests {
        private IWorkflowStateProvider CreateSut() {
            return new WorkflowStateProvider( mIssueProvider, mStateProvider );
        }

        [Fact]
        public async void DeletedStateAffectsIssues() {
            var states = await CreateSomeStates( 3 );
            var issues = await CreateSomeIssues( 3, states );
            using var sut = CreateSut();

            var deleteResult = await sut.DeleteState( states[1] );
            var getIssue1Result = await mIssueProvider.GetIssue( issues[1].EntityId );
            var listResult = await sut.GetStates( SnProject.Default );

            deleteResult.IfLeft( error => error.Should().BeNull( "should be no error deleting a state" ));
            getIssue1Result.IfLeft( error => error.Should().BeNull( "error retrieving issue 1" ));
            getIssue1Result.IfRight( issue => issue.WorkflowStateId.Should().BeEquivalentTo( EntityId.Default, "state should be removed" ));
            listResult.IfRight( list => list.Length().Should().Be( 2, "one state should be deleted" ));
        }
    }
}
