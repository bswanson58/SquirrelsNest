using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Server.Features.Projects;
using SquirrelsNest.Pecan.Shared.Dto.Issues;

namespace SquirrelsNest.Pecan.Server.Features.Issues {
    [Authorize]
    [Route( UpdateIssueRequest.Route )]
    public class UpdateIssueEndpoint : EndpointBaseAsync
        .WithRequest<UpdateIssueRequest>
        .WithActionResult<UpdateIssueResponse> {

        private readonly IIssueProvider                     mIssueProvider;
        private readonly IProjectProvider                   mProjectProvider;
        private readonly ICompositeIssueBuilder             mIssueBuilder;
        private readonly ICompositeProjectBuilder           mProjectBuilder;
        private readonly IValidator<UpdateIssueRequest>     mValidator;

        public UpdateIssueEndpoint( IIssueProvider issueProvider, IProjectProvider projectProvider,
                                    ICompositeIssueBuilder issueBuilder, ICompositeProjectBuilder projectBuilder, 
                                    IValidator<UpdateIssueRequest> validator ) {
            mIssueProvider = issueProvider;
            mProjectProvider = projectProvider;
            mIssueBuilder = issueBuilder;
            mProjectBuilder = projectBuilder;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<UpdateIssueResponse>> HandleAsync(
            [FromBody]UpdateIssueRequest request, 
            CancellationToken cancellationToken = new () ) {
            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return new ActionResult<UpdateIssueResponse>( new UpdateIssueResponse( validation ));
                }

                var project = await mProjectProvider.GetById( request.ProjectId );

                if( project == null ) {
                    return new ActionResult<UpdateIssueResponse>( 
                        new UpdateIssueResponse( "Project for issue to be created is not valid" ));
                }

                var issue = await mIssueProvider.GetById( request.IssueId );

                if( issue == null ) {
                    return new ActionResult<UpdateIssueResponse>( 
                        new UpdateIssueResponse( "Issue to be updated could not be located." ));
                }

                var compositeProject = await mProjectBuilder.BuildComposite( project, cancellationToken );

                issue = issue.With( title: request.Title, description: request.Description )
                    .With( ComponentValidator.ValidateComponent( compositeProject, request.ComponentId ))
                    .With( ComponentValidator.ValidateIssueType( compositeProject, request.IssueTypeId ))
                    .With( ComponentValidator.ValidateWorkflowState( compositeProject, request.WorkflowStateId ))
                    .With( ComponentValidator.ValidateAssignedUser( compositeProject, request.AssignedUserId ));

                issue = await mIssueProvider.Update( issue );

                if( issue == null ) {
                    return new ActionResult<UpdateIssueResponse>( 
                        new UpdateIssueResponse( "Issue could not be updated." ));
                }

                return new ActionResult<UpdateIssueResponse>( 
                    new UpdateIssueResponse( await mIssueBuilder.BuildComposite( issue )));
            }
            catch( Exception ex ) {
                return new ActionResult<UpdateIssueResponse>( new UpdateIssueResponse( ex ));
            }
        }
    }
}
