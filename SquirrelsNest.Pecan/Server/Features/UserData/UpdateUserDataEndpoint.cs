using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.UserData;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.UserData {
    [Authorize]
    [Route( UpdateUserDataRequest.Route )]
    public class UpdateUserDataEndpoint : EndpointBaseAsync 
        .WithRequest<UpdateUserDataRequest>
        .WithActionResult<UpdateUserDataResponse> {

        private readonly IUserDataProvider                  mUserDataProvider;
        private readonly IUserProvider                      mUserProvider;
        private readonly IValidator<UpdateUserDataRequest>  mValidator;

        public UpdateUserDataEndpoint( IUserDataProvider userDataProvider, IUserProvider userProvider, IValidator<UpdateUserDataRequest> validator ) {
            mUserDataProvider = userDataProvider;
            mUserProvider = userProvider;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<UpdateUserDataResponse>> HandleAsync( 
            [FromBody] UpdateUserDataRequest request,
            CancellationToken cancellationToken = new () ) {
            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return new ActionResult<UpdateUserDataResponse>( new UpdateUserDataResponse( validation ));
                }

                var user = await mUserProvider.GetFromContext( HttpContext );

                if( user == null ) {
                    return Ok( new UpdateUserDataResponse( "User for the data could not be located" ));
                }

                var userData = new SnUserData( user.EntityId, request.DataType, request.Data );
                var retValue = await mUserDataProvider.UpdateUserData( userData );

                return Ok( new UpdateUserDataResponse( retValue ));
            }
            catch( Exception ex ) {
                return Ok( new UpdateUserDataResponse( ex ));
            }
        }
    }
}
