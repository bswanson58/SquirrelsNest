using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.Issues;

namespace SquirrelsNest.Pecan.Server.Features.Issues {
    [Authorize]
    [Route( DeleteIssueRequest.Route )]
    public class DeleteIssueEndpoint : EndpointBaseAsync
        .WithRequest<DeleteIssueRequest>
        .WithActionResult<DeleteIssueResponse> {

        private readonly IIssueProvider                     mIssueProvider;
        private readonly IValidator<DeleteIssueRequest>     mValidator;

        public DeleteIssueEndpoint( IIssueProvider issueProvider, IValidator<DeleteIssueRequest> validator ) {
            mIssueProvider = issueProvider;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<DeleteIssueResponse>> HandleAsync( 
            [FromBody] DeleteIssueRequest request, 
            CancellationToken cancellationToken = new () ) {
            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return new ActionResult<DeleteIssueResponse>( new DeleteIssueResponse( validation ));
                }

                await mIssueProvider.Delete( request.Issue.EntityId );

                return Ok( new DeleteIssueResponse( request.Issue ));
            }
            catch( Exception ex ) {
                return Ok( new DeleteIssueResponse( ex ));
            }
        }
    }
}
