using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SquirrelsNest.Pecan.Shared.Dto.Auth;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Features.Auth {
    [Route( LoginUserInput.Route )]
    public class LoginUser : EndpointBaseAsync
        .WithRequest<LoginUserInput>
        .WithActionResult<LoginUserResponse> {

        private readonly    UserManager<IdentityUser>   mUserManager;
        private readonly    SignInManager<IdentityUser> mSignInManager;
        private readonly    IConfiguration              mConfiguration;

        public LoginUser( UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
                          IConfiguration configuration ) {
            mUserManager = userManager;
            mSignInManager = signInManager;
            mConfiguration = configuration;
        }

        [HttpPost]
        public override async Task<ActionResult<LoginUserResponse>> HandleAsync( 
                    [FromBody] LoginUserInput request, 
                    CancellationToken cancellationToken = new()) {
            var result = await mSignInManager.PasswordSignInAsync( request.LoginName, request.Password,
                                                                   isPersistent: false, lockoutOnFailure: false );

            if( result.Succeeded ) {
                var user = await mUserManager.FindByNameAsync( request.LoginName );

                if( user != null ) {
                    var claims = BuildUserClaims( user );
                    var dbClaims = await mUserManager.GetClaimsAsync( user );

                    claims.AddRange( dbClaims );

                    return Ok( BuildToken( claims ));
                }

                return Ok( new LoginUserResponse( "User cannot be located" ));
            }

            if( result.IsLockedOut ) {
                return Ok( new LoginUserResponse( "User is locked out." ));
            }

            if( result.IsNotAllowed ) {
                return Ok( new LoginUserResponse( "User is not allowed to login" ));
            }

            return Ok( new LoginUserResponse( result.ToString()));
        }

        private LoginUserResponse BuildToken( IEnumerable<Claim> claims ) {
            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( mConfiguration["JwtKey"] ?? String.Empty ));
            var credentials = new SigningCredentials( key, SecurityAlgorithms.HmacSha256 );

            var expiration = DateTimeProvider.Instance.CurrentDateTime.AddYears( 1 );

            var token = new JwtSecurityToken( issuer: null, audience: null, claims: claims, expires: expiration, 
                                              signingCredentials: credentials );

            return new LoginUserResponse( new JwtSecurityTokenHandler().WriteToken( token ), expiration );
        }

        private List<Claim> BuildUserClaims( IdentityUser user ) {
            return new List<Claim> {
                new( ClaimTypes.Name, user.UserName ?? String.Empty ),
                new( "entityId", user.Id ),
                new( ClaimTypes.Email, user.Email ?? String.Empty )
            };
        }
    }
}
