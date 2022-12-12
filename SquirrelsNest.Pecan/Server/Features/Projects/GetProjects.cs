using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Features.Projects.Dto;
using SquirrelsNest.Pecan.Server.Models.Entities;
using SquirrelsNest.Pecan.Server.Support;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Route(Routes.GetProjects)]
    public class GetProjects : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<GetProjectsResponse> {

        [HttpGet]
        public override async Task<ActionResult<GetProjectsResponse>> HandleAsync( CancellationToken token ) {

            var project1 = new SnProject( "Project 1", "P1" );
            var project2 = new SnProject( "Project 2", "P1" );
            var response = new GetProjectsResponse( new []{ project1, project2 } );

            return new ActionResult<GetProjectsResponse>( response );
        }
    }
}
