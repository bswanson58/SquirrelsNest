using System.Linq;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Issues {
    public static class ComponentValidator {
        public static SnComponent ValidateComponent( SnCompositeProject forProject, string componentId ) =>
            forProject.Components.FirstOrDefault( c => c.EntityId.Equals( componentId )) ?? SnComponent.Default;

        public static SnIssueType ValidateIssueType( SnCompositeProject forProject, string issueTypeId ) =>
            forProject.IssueTypes.FirstOrDefault( i => i.EntityId.Equals( issueTypeId )) ?? SnIssueType.Default;

        public static SnWorkflowState ValidateWorkflowState( SnCompositeProject forProject, string stateId ) =>
            forProject.WorkflowStates.FirstOrDefault( s => s.EntityId.Equals( stateId )) ?? SnWorkflowState.Default;

        public static SnUser ValidateAssignedUser( SnCompositeProject forProject, string userId ) =>
            forProject.Users.FirstOrDefault( u => u.EntityId.Equals( userId )) ?? SnUser.Default;
    }
}
