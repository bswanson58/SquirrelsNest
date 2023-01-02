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

        public CompositeIssueBuilder( IComponentProvider componentProvider, IIssueTypeProvider issueTypeProvider,
                                      IReleaseProvider releaseProvider, IWorkflowStateProvider workflowStateProvider ) {
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mReleaseProvider = releaseProvider;
            mWorkflowStateProvider = workflowStateProvider;
        }

        public async Task<SnCompositeIssue> BuildComposite( SnIssue fromIssue ) {
            var component = await mComponentProvider.GetById( fromIssue.ComponentId ) ?? SnComponent.Default;
            var issueType = await mIssueTypeProvider.GetById( fromIssue.IssueTypeId ) ?? SnIssueType.Default;
            var state = await mWorkflowStateProvider.GetById( fromIssue.WorkflowStateId ) ?? SnWorkflowState.Default;
            var release = await mReleaseProvider.GetById( fromIssue.ReleaseId ) ?? SnRelease.Default;

            return new SnCompositeIssue( fromIssue, SnUser.Default, issueType, component, state, release, SnUser.Default );
        }
    }
}
