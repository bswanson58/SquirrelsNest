using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto;

namespace SquirrelsNest.Pecan.Server.Features.Auth {
    [Route(CreateUserInput.Route)]
    public class CreateUser : EndpointBaseAsync
        .WithRequest<CreateUserInput>
        .WithActionResult<CreateUserResponse> {

        private readonly    IDbContext                  mContext;
        private readonly    UserManager<IdentityUser>   mUserManager;
        private readonly    IValidator<CreateUserInput> mValidator;

        public CreateUser( UserManager<IdentityUser> userManager, IDbContext context, IValidator<CreateUserInput> validator ) {
            mUserManager = userManager;
            mValidator = validator;
            mContext = context;
        }

        [HttpPost]
        public override async Task<ActionResult<CreateUserResponse>> HandleAsync(
                    [FromBody] CreateUserInput request, 
                    CancellationToken cancellationToken = new()) {
            try {
                var validation = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validation.IsValid ) {
                    return Ok( new CreateUserResponse( validation ));
                }

                var user = new IdentityUser { UserName = request.Email, Email = request.Email };
                var firstUser = !mContext.Users.Any();
                var result = await mUserManager.CreateAsync( user, request.Password );

                if( result.Succeeded ) {
                    // make the first user to be created an admin
                    if(!firstUser ) {
                        result = await mUserManager.AddClaimAsync( user, new Claim( ClaimTypes.Role, ClaimValues.ClaimRoleAdmin ));
                    }

                    // all users have the user role.
                    if( result.Succeeded ) {
                        result = await mUserManager.AddClaimAsync( user, new Claim( ClaimTypes.Role, ClaimValues.ClaimRoleUser ));
                    }

                    if( result.Succeeded ) {
                        return Ok( new CreateUserResponse());
                    }
                }

                return Ok( new CreateUserResponse( result.Errors.Select( e => e.Description ))); 
            }
            catch( Exception ex ) {
                return Ok( new CreateUserResponse( ex ));
            }
        }
    }
}
