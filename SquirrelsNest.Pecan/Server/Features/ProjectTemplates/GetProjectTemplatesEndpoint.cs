using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.Support;
using SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.ProjectTemplates {
    [Authorize]
    [Route( GetProjectTemplatesRequest.Route )]
    public class GetProjectTemplatesEndpoint : EndpointBaseAsync
        .WithRequest<GetProjectTemplatesRequest>
        .WithActionResult<GetProjectTemplatesResponse> {

        private readonly IProjectTemplateManager                mTemplateManager;
        private readonly IValidator<GetProjectTemplatesRequest> mValidator;

        public GetProjectTemplatesEndpoint( IProjectTemplateManager templateManager, IValidator<GetProjectTemplatesRequest> validator ) {
            mTemplateManager = templateManager;
            mValidator = validator;
        }

        public override async Task<ActionResult<GetProjectTemplatesResponse>> HandleAsync( 
            [FromBody] GetProjectTemplatesRequest request,
            CancellationToken cancellationToken = new () ) {
            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new GetProjectTemplatesResponse( validInput ));
                }

                var templates = ( await mTemplateManager.GetAvailableTemplates()).ToList();
                var templateList = PagedList<SnProjectTemplate>
                    .CreatePagedList( templates
                        .Select( t => new SnProjectTemplate( t.TemplateName, t.TemplateDescription ))
                        .ToList(), 
                        request.PageRequest );

                return Ok( new GetProjectTemplatesResponse( templateList, templateList.PageInformation ));
            }
            catch( Exception ex ) {
                return Ok( new GetProjectTemplatesResponse( ex ));
            }
        }
    }
}
