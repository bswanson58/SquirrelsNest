using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto;

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
        public override async Task<ActionResult<GetProjectsResponse>> HandleAsync( CancellationToken token = default ) {
            try {
                var projectList = await mProjectProvider.GetAll().ToListAsync( cancellationToken: token );

                return new ActionResult<GetProjectsResponse>( new GetProjectsResponse( projectList ));
            }
            catch( Exception ex ) {
                return new ActionResult<GetProjectsResponse>( new GetProjectsResponse( ex ));
            }
        }
    }
}
