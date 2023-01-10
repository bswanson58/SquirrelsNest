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
    [Route( EditProjectRequest.Route )]
    public class EditProjectEndpoint : EndpointBaseAsync
        .WithRequest<EditProjectRequest>
        .WithActionResult<EditProjectResponse> {

        private readonly IProjectProvider               mProjectProvider;
        private readonly ICompositeProjectBuilder       mProjectBuilder;
        private readonly IValidator<EditProjectRequest> mValidator;

        public EditProjectEndpoint( IProjectProvider projectProvider, ICompositeProjectBuilder projectBuilder, IValidator<EditProjectRequest> validator ) {
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<EditProjectResponse>> HandleAsync(
            [FromBody] EditProjectRequest request, 
            CancellationToken cancellationToken = new () ) {
            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new EditProjectResponse( validInput ));
                }

                var project = await mProjectProvider.GetById( request.ProjectId );

                if( project != null ) {
                    project = project.With( name: request.Name, description: request.Description, 
                                            issuePrefix: request.IssuePrefix, nextIssueNumber: request.NextIssueNumber );

                    project = await mProjectProvider.Update( project );

                    if( project != null ) {
                        return Ok( new EditProjectResponse( await mProjectBuilder.BuildComposite( project, cancellationToken )));
                    }

                    return Ok( new EditProjectResponse( "Project could not be updated." ));
                }

                return Ok( new EditProjectResponse( "The project to be edited could not be located." ));
            }
            catch( Exception ex ) {
                return Ok( new EditProjectResponse( ex ));
            }
        }
    }
}
