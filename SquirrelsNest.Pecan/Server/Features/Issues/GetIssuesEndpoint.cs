using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.Issues;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Issues {
    [Authorize]
    [Route( GetIssuesRequest.Route )]
    public class GetIssuesEndpoint : EndpointBaseAsync
        .WithRequest<GetIssuesRequest>
        .WithActionResult<GetIssuesResponse> {

        private readonly IIssueProvider         mIssueProvider;
        private readonly IComponentProvider     mComponentProvider;
        private readonly IIssueTypeProvider     mIssueTypeProvider;
        private readonly IReleaseProvider       mReleaseProvider;
        private readonly IWorkflowStateProvider mWorkflowStateProvider;

        public GetIssuesEndpoint( IIssueProvider issueProvider, IComponentProvider componentProvider, IIssueTypeProvider issueTypeProvider, IReleaseProvider releaseProvider, IWorkflowStateProvider workflowStateProvider ) {
            mIssueProvider = issueProvider;
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mReleaseProvider = releaseProvider;
            mWorkflowStateProvider = workflowStateProvider;
        }

        public override async Task<ActionResult<GetIssuesResponse>> HandleAsync( 
            [FromBody] GetIssuesRequest request, 
            CancellationToken cancellationToken = new () ) {

            var issueList = new List<SnCompositeIssue>();
            var issues = await mIssueProvider.GetAll().ToListAsync( cancellationToken );

            foreach( var issue in issues ) {
                var component = await mComponentProvider.GetById( issue.ComponentId ) ?? SnComponent.Default;
                var issueType = await mIssueTypeProvider.GetById( issue.IssueTypeId ) ?? SnIssueType.Default;
                var state = await mWorkflowStateProvider.GetById( issue.WorkflowStateId ) ?? SnWorkflowState.Default;
                var release = await mReleaseProvider.GetById( issue.ReleaseId ) ?? SnRelease.Default;

                issueList.Add( new SnCompositeIssue( issue, SnUser.Default, issueType, component, state, release, SnUser.Default ));
            }

            return new ActionResult<GetIssuesResponse>( new GetIssuesResponse( issueList ));
        }
    }
}
