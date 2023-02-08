using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.UserData;

namespace SquirrelsNest.Pecan.Server.Features.UserData {
    [Authorize]
    [Route( GetUserDataRequest.Route )]
    public class GetUserDataEndpoint : EndpointBaseAsync
        .WithRequest<GetUserDataRequest>
        .WithActionResult<GetUserDataResponse> {

        private readonly IUserDataProvider              mUserDataProvider;
        private readonly IUserProvider                  mUserProvider;
        private readonly IValidator<GetUserDataRequest> mValidator;

        public GetUserDataEndpoint( IUserDataProvider userDataProvider, IUserProvider userProvider, IValidator<GetUserDataRequest> validator ) {
            mUserDataProvider = userDataProvider;
            mUserProvider = userProvider;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<GetUserDataResponse>> HandleAsync( 
            [FromBody] GetUserDataRequest request, 
            CancellationToken cancellationToken = new () ) {
            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return new ActionResult<GetUserDataResponse>( new GetUserDataResponse( validation ));
                }

                var user = await mUserProvider.GetFromContext( HttpContext );

                if( user == null ) {
                    return Ok( new GetUserDataResponse( "User for the data could not be located" ));
                }

                var userData = await mUserDataProvider.GetUserData( user, request.DataType );

                return Ok( new GetUserDataResponse( request.DataType, userData.Data ));

            }
            catch( Exception ex ) {
                return Ok( new GetUserDataResponse( ex ));
            }
        }
    }
}
