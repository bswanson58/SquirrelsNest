using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class IssueBuilder : IIssueBuilder {
        private readonly IProjectProvider           mProjectProvider;
        private readonly IIssueTypeProvider         mIssueTypeProvider;
        private readonly IComponentProvider         mComponentProvider;
        private readonly IWorkflowStateProvider     mStateProvider;
        private readonly IUserProvider              mUserProvider;

        public IssueBuilder( IProjectProvider projectProvider, IIssueTypeProvider issueTypeProvider, IWorkflowStateProvider stateProvider,
                             IComponentProvider componentProvider, IUserProvider userProvider ) {
            mProjectProvider = projectProvider;
            mIssueTypeProvider = issueTypeProvider;
            mStateProvider = stateProvider;
            mComponentProvider = componentProvider;
            mUserProvider = userProvider;
        }

        public async Task<Either<Error, CompositeIssue>> BuildCompositeIssue( SnIssue forIssue ) {
            if( forIssue == null ) {
                throw new ArgumentNullException( nameof( forIssue ));
            }

            var project = ( await mProjectProvider.GetProject( forIssue.ProjectId ).ConfigureAwait( false ))
                .IfLeft( SnProject.Default );
            var issueType = ( await mIssueTypeProvider.GetIssue( forIssue.IssueTypeId ).ConfigureAwait( false ))
                .IfLeft( SnIssueType.Default );
            var component = ( await mComponentProvider.GetComponent( forIssue.ComponentId ).ConfigureAwait( false ))
                .IfLeft( SnComponent.Default );
            var state = ( await mStateProvider.GetState( forIssue.WorkflowStateId ).ConfigureAwait( false ))
                .IfLeft( SnWorkflowState.Default );
            var enteredBy = ( await mUserProvider.GetUser( forIssue.EnteredById ).ConfigureAwait( false ))
                .IfLeft( SnUser.Default );
            var assignedTo = ( await mUserProvider.GetUser( forIssue.AssignedToId ).ConfigureAwait( false ))
                .IfLeft( SnUser.Default );
            
            return new CompositeIssue( project, forIssue, issueType, enteredBy, component, state, assignedTo );
        }
    }
}
