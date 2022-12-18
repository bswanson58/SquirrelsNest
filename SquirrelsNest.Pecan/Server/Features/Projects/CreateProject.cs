using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
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

        private readonly IProjectProvider   mProjectProvider;

        public CreateProject( IProjectProvider projectProvider ) {
            mProjectProvider = projectProvider;
        }

        [HttpPost]
        public override async Task<ActionResult<CreateProjectResponse>> HandleAsync( 
            [FromBody] CreateProjectInput request,
            CancellationToken cancellationToken = new()) {

            try {
                var project = new SnProject( request.Name, request.IssuePrefix ).With( description: request.Description );
                var result =  await mProjectProvider.Create( project );

                return new ActionResult<CreateProjectResponse>( new CreateProjectResponse( result ));
            }
            catch( Exception ex ) {
                return new ActionResult<CreateProjectResponse>( new CreateProjectResponse( ex ));
            }
        }
    }
}
