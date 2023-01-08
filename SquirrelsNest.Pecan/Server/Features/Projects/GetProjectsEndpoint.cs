using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Server.Database.Support;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Authorize]
    [Route(GetProjectsRequest.Route)]
    public class GetProjectsEndpoint : EndpointBaseAsync
        .WithRequest<GetProjectsRequest>
        .WithActionResult<GetProjectsResponse> {

        private readonly IProjectProvider               mProjectProvider;
        private readonly ICompositeProjectBuilder       mProjectBuilder;
        private readonly IValidator<GetProjectsRequest> mValidator;

        public GetProjectsEndpoint( IProjectProvider projectProvider, ICompositeProjectBuilder projectBuilder,
            IValidator<GetProjectsRequest> validator ) {
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<GetProjectsResponse>> HandleAsync(
            [FromBody] GetProjectsRequest request,
            CancellationToken token = new()) {
            try {
                var validation = await mValidator.ValidateAsync( request, token );

                if(!validation.IsValid ) {
                    return new ActionResult<GetProjectsResponse>( new GetProjectsResponse( validation ));
                }

                var projectList = 
                    await PagedList<SnProject>.CreatePagedList( mProjectProvider.GetAll(), request.PageRequest, token );
                var compositeProjects = new List<SnCompositeProject>();

                foreach( var project in projectList ) {
                    compositeProjects.Add( await mProjectBuilder.BuildComposite( project, token ));
                }

                return Ok( new GetProjectsResponse( compositeProjects, projectList.PageInformation ));
            }
            catch( Exception ex ) {
                return Ok( new GetProjectsResponse( ex ));
            }
        }
    }
}
