using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Server.Features.Auth {
    [Authorize]
    [Route( ChangeUserRolesRequest.Route )]
    public class ChangeUserRolesEndpoint : EndpointBaseAsync
        .WithRequest<ChangeUserRolesRequest>
        .WithActionResult<ChangeUserRolesResponse> {

        private readonly IUserService                       mUserService;
        private readonly IUserProvider                      mUserProvider;
        private readonly IValidator<ChangeUserRolesRequest> mValidator;

        public ChangeUserRolesEndpoint( IUserService userService, IUserProvider userProvider,
                                        IValidator<ChangeUserRolesRequest> validator ) {
            mUserService = userService;
            mUserProvider = userProvider;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<ChangeUserRolesResponse>> HandleAsync( 
            [FromBody] ChangeUserRolesRequest request, 
            CancellationToken cancellationToken = new () ) {
            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return Ok( new ChangeUserRolesResponse( validation ));
                }

                await mUserService.UpdateUserRoles( request.User, 
                                                    request.DisableRoles ? new List<string>() : request.Roles );

                var user = await mUserProvider.GetById( request.User.EntityId );

                if( user !=  null ) {
                    return Ok( new ChangeUserRolesResponse( user ));
                }

                return Ok( new ChangeUserRolesResponse( "Resulting user could not be determined." ));
            }
            catch( Exception ex ) {
                return Ok( new ChangeUserRolesResponse( ex ));
            }
        }
    }
}
