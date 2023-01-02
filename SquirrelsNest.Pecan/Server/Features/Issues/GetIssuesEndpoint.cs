using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.Issues;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Issues {
    [Authorize]
    [Route( GetIssuesRequest.Route )]
    public class GetIssuesEndpoint : EndpointBaseAsync
        .WithRequest<GetIssuesRequest>
        .WithActionResult<GetIssuesResponse> {

        private readonly IIssueProvider                 mIssueProvider;
        private readonly IProjectProvider               mProjectProvider;
        private readonly ICompositeIssueBuilder         mIssueBuilder;
        private readonly IValidator<GetIssuesRequest>   mValidator;

        public GetIssuesEndpoint( IIssueProvider issueProvider, IProjectProvider projectProvider,
                                  ICompositeIssueBuilder issueBuilder, IValidator<GetIssuesRequest> validator ) {
            mIssueBuilder = issueBuilder;
            mValidator = validator;
            mProjectProvider = projectProvider;
            mIssueProvider = issueProvider;
        }

        [HttpPost]
        public override async Task<ActionResult<GetIssuesResponse>> HandleAsync( 
            [FromBody] GetIssuesRequest request, 
            CancellationToken cancellationToken = new () ) {

            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return new ActionResult<GetIssuesResponse>( new GetIssuesResponse( validation ));
                }

                var issueList = new List<SnCompositeIssue>();

                var project = await mProjectProvider.GetById( request.ProjectId );

                if( project == null ) {
                    return new ActionResult<GetIssuesResponse>( 
                        new GetIssuesResponse( "Project for issue list could not be located" ));
                }
                var issues = await mIssueProvider.GetAll().ToListAsync( cancellationToken );

                foreach( var issue in issues ) {
                    issueList.Add( await mIssueBuilder.BuildComposite( issue ));
                }

                return new ActionResult<GetIssuesResponse>( new GetIssuesResponse( issueList ));
            }
            catch( Exception ex ) {
                return new ActionResult<GetIssuesResponse>( new GetIssuesResponse( ex ));
            }
        }
    }
}
