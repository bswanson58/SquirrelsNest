using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Authorize]
    [Route( DeleteProjectRequest.Route )]
    public class DeleteProjectEndpoint : EndpointBaseAsync
        .WithRequest<DeleteProjectRequest>
        .WithActionResult<DeleteProjectResponse> {

        private readonly IProjectProvider                   mProjectProvider;
        private readonly ICompositeProjectBuilder           mProjectBuilder;
        private readonly IIssueProvider                     mIssueProvider;
        private readonly IComponentProvider                 mComponentProvider;
        private readonly IIssueTypeProvider                 mIssueTypeProvider;
        private readonly IReleaseProvider                   mReleaseProvider;
        private readonly IWorkflowStateProvider             mStateProvider;
        private readonly IValidator<DeleteProjectRequest>   mValidator;

        public DeleteProjectEndpoint( IProjectProvider projectProvider, IComponentProvider componentProvider, 
                                      IIssueTypeProvider issueTypeProvider, IReleaseProvider releaseProvider, 
                                      IWorkflowStateProvider stateProvider, ICompositeProjectBuilder projectBuilder,
                                      IIssueProvider issueProvider, IValidator<DeleteProjectRequest> validator ) {
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mIssueProvider = issueProvider;
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mReleaseProvider = releaseProvider;
            mStateProvider = stateProvider;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<DeleteProjectResponse>> HandleAsync( 
            [FromBody] DeleteProjectRequest request,
            CancellationToken cancellationToken = new () ) {
            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new DeleteProjectResponse( validInput ));
                }

                var project = await mProjectProvider.GetById( request.Project.EntityId );

                if( project == null ) {
                    return Ok( new DeleteProjectResponse( "Project to be deleted was not located" ));
                }

                var compositeProject = await mProjectBuilder.BuildComposite( project, cancellationToken );

                foreach( var issue in mIssueProvider.GetAll( project )) {
                    await mIssueProvider.Delete( issue );
                }

                foreach( var component in compositeProject.Components ) {
                    await mComponentProvider.Delete( component );
                }

                foreach ( var issueType in compositeProject.IssueTypes ) {
                    await mIssueTypeProvider.Delete( issueType );
                }

                foreach ( var release in compositeProject.Releases ) {
                    await mReleaseProvider.Delete( release );
                }

                foreach ( var state in compositeProject.WorkflowStates ) {
                    await mStateProvider.Delete( state );
                }

                await mProjectProvider.Delete( project );

                return Ok( new DeleteProjectResponse( compositeProject ));
            }
            catch( Exception ex ) {
                return Ok( new DeleteProjectResponse( ex ));
            }
        }
    }
}
