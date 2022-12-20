using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Route(Routes.GetProjects)]
    public class GetProjects : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<GetProjectsResponse> {

        private readonly IProjectProvider   mProjectProvider;

        public GetProjects( IProjectProvider projectProvider ) {
            mProjectProvider = projectProvider;
        }

        [HttpGet]
        public override async Task<ActionResult<GetProjectsResponse>> HandleAsync( CancellationToken token = new()) {
            try {
                var projectList = await mProjectProvider.GetAll().ToListAsync( cancellationToken: token );

                return Ok( new GetProjectsResponse( projectList ));
            }
            catch( Exception ex ) {
                return Ok( new GetProjectsResponse( ex ));
            }
        }
    }
}
