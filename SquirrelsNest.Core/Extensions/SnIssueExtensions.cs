using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.Extensions {
    public static class SnIssueExtensions {
        public static SnIssue ToggleCompletedState( this SnIssue issue, IReadOnlyList<SnWorkflowState> workflowStates ) {
            var state = workflowStates.FirstOrDefault( s => s.EntityId.Equals( issue.WorkflowStateId ), SnWorkflowState.Default );
            var newState = state.Category == StateCategory.Completed || state.Category == StateCategory.Terminal ?
                    workflowStates.FirstOrDefault( 
                        s => s.Category == StateCategory.Initial, 
                        workflowStates.FirstOrDefault( SnWorkflowState.Default )) :
                    workflowStates.FirstOrDefault( 
                        s => s.Category == StateCategory.Completed, 
                        workflowStates.FirstOrDefault( s => s.Category == StateCategory.Terminal, SnWorkflowState.Default ));

            return issue.With( newState );
        }
    }
}
