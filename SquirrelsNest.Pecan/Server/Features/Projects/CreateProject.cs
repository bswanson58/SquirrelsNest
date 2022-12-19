using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Route(Routes.CreateProject)]
    public class CreateProject : EndpointBaseAsync
        .WithRequest<CreateProjectInput>
        .WithActionResult<CreateProjectResponse> {

        private readonly IProjectProvider               mProjectProvider;
        private readonly IValidator<CreateProjectInput> mInputValidator;

        public CreateProject( IProjectProvider projectProvider, IValidator<CreateProjectInput> inputValidator ) {
            mProjectProvider = projectProvider;
            mInputValidator = inputValidator;
        }

        [HttpPost]
        public override async Task<ActionResult<CreateProjectResponse>> HandleAsync( 
            [FromBody] CreateProjectInput request,
            CancellationToken cancellationToken = new()) {

            try {
                var validInput = await mInputValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new CreateProjectResponse( validInput ));
                }

                var project = new SnProject( request.Name, request.IssuePrefix ).With( description: request.Description );
                var result =  await mProjectProvider.Create( project );

                return Ok( new CreateProjectResponse( result ));
            }
            catch( Exception ex ) {
                return Ok( new CreateProjectResponse( ex ));
            }
        }
    }
}
