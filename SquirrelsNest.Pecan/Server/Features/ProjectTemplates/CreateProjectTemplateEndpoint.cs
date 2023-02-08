using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.ProjectTemplates {
    [Authorize]
    [Route( CreateProjectTemplateRequest.Route )]
    public class CreateProjectTemplateEndpoint : EndpointBaseAsync
        .WithRequest<CreateProjectTemplateRequest>
        .WithActionResult<CreateProjectTemplateResponse> {

        private readonly IProjectTemplateManager                    mTemplateManager;
        private readonly IValidator<CreateProjectTemplateRequest>   mValidator;

        public CreateProjectTemplateEndpoint( IProjectTemplateManager templateManager, IValidator<CreateProjectTemplateRequest> validator ) {
            mTemplateManager = templateManager;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<CreateProjectTemplateResponse>> HandleAsync( 
            [FromBody] CreateProjectTemplateRequest request, 
            CancellationToken cancellationToken = new () ) {
            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new CreateProjectTemplateResponse( validInput ));
                }

                var template = new TemplateParameters {
                    TemplateName = request.TemplateName,
                    TemplateDescription = request.TemplateDescription
                };

                await mTemplateManager.CreateTemplate( request.Project, template );

                return Ok( new CreateProjectTemplateResponse( 
                    new SnProjectTemplate( request.TemplateName, request.TemplateDescription )));
            }
            catch( Exception ex ) {
                return Ok( new CreateProjectTemplateResponse( ex ));
            }
        }
    }
}
