using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Authorize]
    [Route(IssueTypeChangeInput.Route)]
    public class IssueTypeChangeEndpoint : EndpointBaseAsync
        .WithRequest<IssueTypeChangeInput>
        .WithActionResult<IssueTypeChangeResponse> {

        private readonly IIssueTypeProvider                 mIssueTypeProvider;
        private readonly IProjectProvider                   mProjectProvider;
        private readonly IValidator<IssueTypeChangeInput>   mValidator;

        public IssueTypeChangeEndpoint( IIssueTypeProvider issueTypeProvider, IProjectProvider projectProvider,
                                        IValidator<IssueTypeChangeInput> validator) {
            mIssueTypeProvider = issueTypeProvider;
            mProjectProvider = projectProvider;
            mValidator = validator;
        }

        private async Task<bool> IsValidProject( string projectId ) {
            var project = await mProjectProvider.GetById( projectId );

            return project != null;
        }

        private async Task<SnIssueType> AddIssueType( SnIssueType issueType ) {
            return await mIssueTypeProvider.Create( issueType );
        }

        private async Task<SnIssueType> UpdateIssueType( SnIssueType issueType ) {
            var updatedIssueType = await mIssueTypeProvider.Update( issueType );

            if( updatedIssueType != null ) {
                return updatedIssueType;
            }

            throw new ApplicationException( "IssueType to update was not located." );
        }

        private async Task<SnIssueType> DeleteIssueType( SnIssueType issueType ) {
             await mIssueTypeProvider.Delete( issueType );

             return issueType;
        }

        [HttpPost]
        public override async Task<ActionResult<IssueTypeChangeResponse>> HandleAsync( 
            [FromBody]IssueTypeChangeInput request,
            CancellationToken cancellationToken = new ()) {

            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new IssueTypeChangeResponse( validInput ));
                }

                if(!( await IsValidProject( request.IssueType.ProjectId ))) {
                    return Ok( new IssueTypeChangeResponse( "Invalid project specified in issue type" ));
                }

                switch ( request.ChangeType ) {
                    case EntityChangeType.Add:
                        return Ok( new IssueTypeChangeResponse( await AddIssueType( request.IssueType ), request.ChangeType ));

                    case EntityChangeType.Update:
                        return Ok( new IssueTypeChangeResponse( await UpdateIssueType( request.IssueType ), request.ChangeType ));

                    case EntityChangeType.Delete:
                        return Ok( new IssueTypeChangeResponse( await DeleteIssueType( request.IssueType ), request.ChangeType ));

                    default:
                        return Ok( new IssueTypeChangeResponse( "Unknown change type requested" ));
                }

            }
            catch( Exception ex ) {
                return Ok( new IssueTypeChangeResponse( ex ));

            }
        }
    }
}
