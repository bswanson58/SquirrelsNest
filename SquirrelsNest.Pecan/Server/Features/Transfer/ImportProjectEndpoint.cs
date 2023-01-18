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
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Server.Features.Transfer {
    [Authorize]
    [Route( ImportProjectRequest.Route )]
    public class ImportProjectEndpoint : EndpointBaseAsync
        .WithRequest<ImportProjectRequest>
        .WithActionResult<ImportProjectResponse> {

        private readonly IImportManager                     mImportManager;
        private readonly IUserProvider                      mUserProvider;
        private readonly IValidator<ImportProjectRequest>   mValidator;

        public ImportProjectEndpoint( IImportManager importManager, IUserProvider userProvider,
                                      IValidator<ImportProjectRequest> validator ) {
            mImportManager = importManager;
            mValidator = validator;
            mUserProvider = userProvider;
        }

        public override async Task<ActionResult<ImportProjectResponse>> HandleAsync(
            [FromBody] ImportProjectRequest request,
            CancellationToken cancellationToken = new()) {
            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new ImportProjectResponse( validInput ));
                }

                var user = await mUserProvider.GetFromContext( HttpContext );

                if( user == null ) {
                    return Ok( new ImportProjectResponse( "User for the data could not be located" ));
                }

                var importContent = Encoding.UTF8.GetString( Convert.FromBase64String( request.ImportContent ));
                using var stream = new MemoryStream( Encoding.UTF8.GetBytes( importContent ));

                var project = await mImportManager.ImportProject( stream, request, user, cancellationToken );

                return Ok( new ImportProjectResponse( project ));
            }
            catch( Exception ex ) {
                return Ok( new ImportProjectResponse( ex ));
            }
        }
    }
}
