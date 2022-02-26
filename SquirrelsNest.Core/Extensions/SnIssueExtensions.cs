using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.Extensions {
    public static class SnIssueExtensions {
        public static SnIssue ToggleCompletedState( this SnIssue issue, IReadOnlyList<SnWorkflowState> workflowStates ) {
            var state = workflowStates.FirstOrDefault( s => s.EntityId.Equals( issue.WorkflowStateId ), SnWorkflowState.Default );
            var newState = state.IsFinalState || state.IsTerminalState ?
                    workflowStates.FirstOrDefault( s => s.IsInitialState, workflowStates.FirstOrDefault( SnWorkflowState.Default )) :
                    workflowStates.FirstOrDefault( s => s.IsFinalState, workflowStates.FirstOrDefault( s => s.IsTerminalState, SnWorkflowState.Default ));

            return issue.With( newState );
        }
    }
}
