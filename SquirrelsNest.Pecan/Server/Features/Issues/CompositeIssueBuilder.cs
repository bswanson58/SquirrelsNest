using System.Threading.Tasks;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Issues {
    public interface ICompositeIssueBuilder {
        Task<SnCompositeIssue>  BuildComposite( SnIssue fromIssue );
    }

    public class CompositeIssueBuilder : ICompositeIssueBuilder {
        private readonly IComponentProvider     mComponentProvider;
        private readonly IIssueTypeProvider     mIssueTypeProvider;
        private readonly IReleaseProvider       mReleaseProvider;
        private readonly IWorkflowStateProvider mWorkflowStateProvider;
        private readonly IUserProvider          mUserProvider;

        public CompositeIssueBuilder( IComponentProvider componentProvider, IIssueTypeProvider issueTypeProvider,
                                      IReleaseProvider releaseProvider, IWorkflowStateProvider workflowStateProvider,
                                      IUserProvider userProvider ) {
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mReleaseProvider = releaseProvider;
            mWorkflowStateProvider = workflowStateProvider;
            mUserProvider = userProvider;
        }

        public async Task<SnCompositeIssue> BuildComposite( SnIssue fromIssue ) {
            var component = await mComponentProvider.GetById( fromIssue.ComponentId ) ?? SnComponent.Default;
            var issueType = await mIssueTypeProvider.GetById( fromIssue.IssueTypeId ) ?? SnIssueType.Default;
            var state = await mWorkflowStateProvider.GetById( fromIssue.WorkflowStateId ) ?? SnWorkflowState.Default;
            var release = await mReleaseProvider.GetById( fromIssue.ReleaseId ) ?? SnRelease.Default;
            var assignedTo = await mUserProvider.GetById( fromIssue.AssignedToId ) ?? SnUser.Default;
            var enteredBy = await mUserProvider.GetById( fromIssue.EnteredById ) ?? SnUser.Default;

            return new SnCompositeIssue( fromIssue, enteredBy, issueType, component, state, release, assignedTo );
        }
    }
}
