using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Features.Projects;
using SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates;

namespace SquirrelsNest.Pecan.Server.Features.ProjectTemplates {
    [Authorize]
    [Route( CreateProjectFromTemplateRequest.Route )]
    public class CreateProjectFromTemplateEndpoint : EndpointBaseAsync
        .WithRequest<CreateProjectFromTemplateRequest>
        .WithActionResult<CreateProjectFromTemplateResponse> {

        private readonly IProjectTemplateManager                        mTemplateManager;
        private readonly ICompositeProjectBuilder                       mProjectBuilder;
        private readonly IValidator<CreateProjectFromTemplateRequest>   mValidator;

        public CreateProjectFromTemplateEndpoint( IProjectTemplateManager templateManager, ICompositeProjectBuilder projectBuilder, IValidator<CreateProjectFromTemplateRequest> validator ) {
            mTemplateManager = templateManager;
            mProjectBuilder = projectBuilder;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<CreateProjectFromTemplateResponse>> HandleAsync(
            [FromBody] CreateProjectFromTemplateRequest request, 
            CancellationToken cancellationToken = new () ) {
            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new CreateProjectFromTemplateResponse( validInput ));
                }

                var template = ( await mTemplateManager
                    .GetAvailableTemplates())
                    .FirstOrDefault( t => t.TemplateName.Equals( request.TemplateName ));

                if( template == null ) {
                    return Ok( new CreateProjectFromTemplateResponse( "Template could not be located" ));
                }

                var projectParameters = new ProjectParameters 
                    { ProjectName = request.ProjectName, ProjectDescription = request.ProjectDescription };
                var project = await mTemplateManager.CreateProject( template, projectParameters );
                var compositeProject = await mProjectBuilder.BuildComposite( project, cancellationToken );

                return Ok( new CreateProjectFromTemplateResponse( compositeProject ));
            }
            catch( Exception ex ) {
                return Ok( new CreateProjectFromTemplateResponse( ex ));
            }
        }
    }
}
