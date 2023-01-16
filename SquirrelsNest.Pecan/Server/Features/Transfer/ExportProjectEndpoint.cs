using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Server.Features.Projects;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Server.Features.Transfer {
    [Authorize]
    [Route( ExportProjectRequest.Route )]
    public class ExportProjectEndpoint : EndpointBaseAsync
        .WithRequest<ExportProjectRequest>
        .WithResult<Stream> {

        private readonly IProjectProvider                   mProjectProvider;
        private readonly ICompositeProjectBuilder           mProjectBuilder;
        private readonly IExportManager                     mExportManager;
        private readonly IValidator<ExportProjectRequest>   mValidator;

        public ExportProjectEndpoint( IProjectProvider projectProvider, ICompositeProjectBuilder projectBuilder,
                                      IExportManager exportManager, IValidator<ExportProjectRequest> validator ) {
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mExportManager = exportManager;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<Stream> HandleAsync( 
            [FromBody] ExportProjectRequest request, 
            CancellationToken cancellationToken = new () ) {
            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return new MemoryStream( Encoding.UTF8.GetBytes( "Request is not valid" ));
                }

                var project = await mProjectProvider.GetById( request.ProjectId );

                if( project == null ) {
                    return new MemoryStream( Encoding.UTF8.GetBytes( "Project to be exported is not valid" ));
                }

                var compositeProject = await mProjectBuilder.BuildComposite( project, cancellationToken );

                return await mExportManager.ExportProject( compositeProject, request.IncludeCompletedIssues );
            }
            catch( Exception ex ) {
                return new MemoryStream( Encoding.UTF8.GetBytes( ex.Message ));
            }
        }
    }
}
