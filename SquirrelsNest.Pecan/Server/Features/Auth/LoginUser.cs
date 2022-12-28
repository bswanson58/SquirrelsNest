using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Server.Features.Auth {
    [AllowAnonymous]
    [Route( LoginUserInput.Route )]
    public class LoginUser : EndpointBaseAsync
        .WithRequest<LoginUserInput>
        .WithActionResult<LoginUserResponse> {

        private readonly UserManager<DbUser>        mUserManager;
        private readonly ITokenBuilder              mTokenBuilder;
        private readonly IValidator<LoginUserInput> mValidator;

        public LoginUser( UserManager<DbUser> userManager, ITokenBuilder tokenBuilder, IValidator<LoginUserInput> validator ) {
            mUserManager = userManager;
            mTokenBuilder = tokenBuilder;
            mValidator = validator;
        }

        [HttpPost]
        public override async Task<ActionResult<LoginUserResponse>> HandleAsync( 
                    [FromBody] LoginUserInput request, 
                    CancellationToken cancellationToken = new()) {
            var validation = await mValidator.ValidateAsync( request, cancellationToken );

            if(!validation.IsValid ) {
                return Ok( new LoginUserResponse( validation ));
            }

            var user = await mUserManager.FindByNameAsync( request.LoginName );

            if(( user == null ) ||
               (!await mUserManager.CheckPasswordAsync( user, request.Password ))) {
                return Unauthorized( new LoginUserResponse( "Invalid Authentication" ));
            }

            return Ok( await BuildResponse( user ));
        }

        private async Task<LoginUserResponse> BuildResponse( DbUser user ) {
            var token = await mTokenBuilder.GenerateToken( user );

            user.RefreshToken = mTokenBuilder.GenerateRefreshToken();
            user.RefreshTokenExpiration = mTokenBuilder.TokenExpiration();

            await mUserManager.UpdateAsync( user );

            return new LoginUserResponse( token, user.RefreshToken, user.RefreshTokenExpiration );
        }
    }
}
