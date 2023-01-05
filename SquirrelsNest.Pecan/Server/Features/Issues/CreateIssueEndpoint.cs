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
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Issues {
    [Authorize]
    [Route( CreateIssueRequest.Route )]
    public class CreateIssueEndpoint : EndpointBaseAsync 
        .WithRequest<CreateIssueRequest>
        .WithActionResult<CreateIssueResponse> {

        private readonly IIssueProvider                     mIssueProvider;
        private readonly IProjectProvider                   mProjectProvider;
        private readonly ICompositeIssueBuilder             mIssueBuilder;
        private readonly ICompositeProjectBuilder           mProjectBuilder;
        private readonly IValidator<CreateIssueRequest>     mValidator;

        public CreateIssueEndpoint( IIssueProvider issueProvider, IProjectProvider projectProvider, 
                                    ICompositeIssueBuilder issueBuilder, ICompositeProjectBuilder projectBuilder,
                                    IValidator<CreateIssueRequest> validator ) {
            mIssueProvider = issueProvider;
            mProjectProvider = projectProvider;
            mIssueBuilder = issueBuilder;
            mProjectBuilder = projectBuilder;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<CreateIssueResponse>> HandleAsync( 
            [FromBody]CreateIssueRequest request,
            CancellationToken cancellationToken = new () ) {
            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return new ActionResult<CreateIssueResponse>( new CreateIssueResponse( validation ));
                }

                var project = await mProjectProvider.GetById( request.ProjectId );

                if( project == null ) {
                    return new ActionResult<CreateIssueResponse>( 
                        new CreateIssueResponse( "Project for issue to be created is not valid" ));
                }

                var compositeProject = await mProjectBuilder.BuildComposite( project, cancellationToken );

                var newIssue = new SnIssue( request.Title, request.Description, project.NextIssueNumber, project.EntityId )
                    .With( ComponentValidator.ValidateComponent( compositeProject, request.ComponentId ))
                    .With( ComponentValidator.ValidateIssueType( compositeProject, request.IssueTypeId ))
                    .With( ComponentValidator.ValidateWorkflowState( compositeProject, request.WorkflowStateId ));

                var result = await mProjectProvider.Update( project.WithNextIssueNumber());

                if( result == null ) {
                    return new ActionResult<CreateIssueResponse>( 
                        new CreateIssueResponse( "Project could not be updated when creating the issue" ));
                }

                var issue = await mIssueProvider.Create( newIssue );

                return new ActionResult<CreateIssueResponse>( 
                    new CreateIssueResponse( await mIssueBuilder.BuildComposite( issue )));
            }
            catch( Exception ex ) {
                return new ActionResult<CreateIssueResponse>( new CreateIssueResponse( ex ));
            }
        }
    }
}
