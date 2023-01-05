using System.Linq;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Issues {
    public static class ComponentValidator {
        public static SnComponent ValidateComponent( SnCompositeProject forProject, string componentId ) =>
            forProject.Components.FirstOrDefault( c => c.EntityId.Equals( componentId )) ?? SnComponent.Default;

        public static string ValidateIssueType( SnCompositeProject forProject, string issueTypeId ) {
            var issueType = forProject.IssueTypes.FirstOrDefault( i => i.EntityId.Equals( issueTypeId )) ?? SnIssueType.Default;

            return issueType.EntityId;
        }

        public static string ValidateWorkflowState( SnCompositeProject forProject, string stateId ) {
            var state = forProject.WorkflowStates.FirstOrDefault( s => s.EntityId.Equals( stateId )) ?? SnWorkflowState.Default;

            return state.EntityId;
        }
    }
}
