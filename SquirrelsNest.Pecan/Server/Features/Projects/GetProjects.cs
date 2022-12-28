using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Authorize]
    [Route(Routes.GetProjects)]
    public class GetProjects : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<GetProjectsResponse> {

        private readonly IProjectProvider       mProjectProvider;
        private readonly IComponentProvider     mComponentProvider;
        private readonly IIssueTypeProvider     mIssueTypeProvider;
        private readonly IWorkflowStateProvider mWorkflowStateProvider;
        private readonly IReleaseProvider       mReleaseProvider;

        public GetProjects( IProjectProvider projectProvider, IComponentProvider componentProvider,
                            IIssueTypeProvider issueTypeProvider, IWorkflowStateProvider workflowStateProvider,
                            IReleaseProvider releaseProvider ) {
            mProjectProvider = projectProvider;
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mWorkflowStateProvider = workflowStateProvider;
            mReleaseProvider = releaseProvider;
        }

        [HttpGet]
        public override async Task<ActionResult<GetProjectsResponse>> HandleAsync( CancellationToken token = new()) {
            try {
                var projectList = await mProjectProvider.GetAll().ToListAsync( cancellationToken: token );
                var compositeProjects = new List<SnCompositeProject>();

                foreach( var project in projectList ) {
                    compositeProjects.Add( new SnCompositeProject( project,
                        await mComponentProvider.GetAll( project ).ToListAsync( token ),
                        await mIssueTypeProvider.GetAll( project ).ToListAsync( token ),
                        await mWorkflowStateProvider.GetAll( project ).ToListAsync( token ),
                        await mReleaseProvider.GetAll( project ).ToListAsync( token )
                        ));
                }

                return Ok( new GetProjectsResponse( compositeProjects ));
            }
            catch( Exception ex ) {
                return Ok( new GetProjectsResponse( ex ));
            }
        }
    }
}
