using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Route(Routes.GetProjects)]
    public class GetProjects : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<GetProjectsResponse> {

        [HttpGet]
        public override async Task<ActionResult<GetProjectsResponse>> HandleAsync( CancellationToken token ) {

            var project1 = new SnProject( "Project 1", "P1" );
            var project2 = new SnProject( "Project 2", "P1" );
            var response = new GetProjectsResponse( new List<SnProject>(){ project1, project2 } );

            return new ActionResult<GetProjectsResponse>( response );
        }
    }
}
