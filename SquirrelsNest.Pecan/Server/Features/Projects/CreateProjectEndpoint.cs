using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Authorize]
    [Route( CreateProjectRequest.Route )]
    public class CreateProjectEndpoint : EndpointBaseAsync
        .WithRequest<CreateProjectRequest>
        .WithActionResult<CreateProjectResponse> {

        private readonly IProjectProvider               mProjectProvider;
        private readonly IValidator<CreateProjectRequest> mInputValidator;

        public CreateProjectEndpoint( IProjectProvider projectProvider, IValidator<CreateProjectRequest> inputValidator ) {
            mProjectProvider = projectProvider;
            mInputValidator = inputValidator;
        }

        [HttpPost]
        public override async Task<ActionResult<CreateProjectResponse>> HandleAsync( 
            [FromBody] CreateProjectRequest request,
            CancellationToken cancellationToken = new()) {

            try {
                var validInput = await mInputValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new CreateProjectResponse( validInput ));
                }

                var project = new SnProject( request.Name, request.IssuePrefix ).With( description: request.Description );
                var result =  await mProjectProvider.Create( project );

                return Ok( new CreateProjectResponse( new SnCompositeProject( result )));
            }
            catch( Exception ex ) {
                return Ok( new CreateProjectResponse( ex ));
            }
        }
    }
}
