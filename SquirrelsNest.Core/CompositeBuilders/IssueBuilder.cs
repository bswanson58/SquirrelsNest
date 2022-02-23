using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class IssueBuilder : IIssueBuilder {
        private readonly IProjectProvider           mProjectProvider;
        private readonly IIssueTypeProvider         mIssueTypeProvider;
        private readonly IWorkflowStateProvider     mStateProvider;

        public IssueBuilder( IProjectProvider projectProvider, IIssueTypeProvider issueTypeProvider, IWorkflowStateProvider stateProvider ) {
            mProjectProvider = projectProvider;
            mIssueTypeProvider = issueTypeProvider;
            mStateProvider = stateProvider;
        }

        private SnProject GetProject( SnIssue forIssue ) {
            return mProjectProvider
                .GetProject( forIssue.ProjectId ).Result
                .IfLeft( SnProject.Default );
        }

        private SnIssueType GetIssueType( SnIssue forIssue ) {
            return mIssueTypeProvider
                .GetIssue( forIssue.IssueTypeId ).Result
                .IfLeft( SnIssueType.Default );
        }

        private SnWorkflowState GetState( SnIssue forIssue ) {
            return mStateProvider
                .GetState( forIssue.WorkflowStateId ).Result
                .IfLeft( SnWorkflowState.Default );
        }

        public CompositeIssue BuildCompositeIssue( SnIssue forIssue ) {
            if( forIssue == null ) {
                throw new ArgumentNullException( nameof( forIssue ));
            }

            return new CompositeIssue( GetProject( forIssue ), forIssue, GetIssueType( forIssue ), GetState( forIssue ));
        }
    }
}
