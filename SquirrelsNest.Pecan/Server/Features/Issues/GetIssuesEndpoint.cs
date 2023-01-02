using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
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

        private readonly IIssueProvider         mIssueProvider;
        private readonly ICompositeIssueBuilder mIssueBuilder;

        public GetIssuesEndpoint( IIssueProvider issueProvider, ICompositeIssueBuilder issueBuilder ) {
            mIssueBuilder = issueBuilder;
            mIssueProvider = issueProvider;
        }

        public override async Task<ActionResult<GetIssuesResponse>> HandleAsync( 
            [FromBody] GetIssuesRequest request, 
            CancellationToken cancellationToken = new () ) {

            try {
                var issueList = new List<SnCompositeIssue>();
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
