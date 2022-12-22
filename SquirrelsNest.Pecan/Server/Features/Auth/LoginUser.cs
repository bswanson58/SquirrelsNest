using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto.Auth;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Features.Auth {
    [Route( LoginUserInput.Route )]
    public class LoginUser : EndpointBaseAsync
        .WithRequest<LoginUserInput>
        .WithActionResult<LoginUserResponse> {

        private readonly    UserManager<IdentityUser>   mUserManager;
        private readonly    IConfiguration              mConfiguration;

        public LoginUser( UserManager<IdentityUser> userManager, IConfiguration configuration ) {
            mUserManager = userManager;
            mConfiguration = configuration;
        }

        [HttpPost]
        public override async Task<ActionResult<LoginUserResponse>> HandleAsync( 
                    [FromBody] LoginUserInput request, 
                    CancellationToken cancellationToken = new()) {
            var user = await mUserManager.FindByNameAsync( request.LoginName );

            if(( user == null ) ||
               (!await mUserManager.CheckPasswordAsync( user, request.Password ))) {
                return Unauthorized( new LoginUserResponse( "Invalid Authentication" ));
            }

            return Ok( BuildToken( await BuildUserClaims( user )));
        }

        private LoginUserResponse BuildToken( IEnumerable<Claim> claims ) {
            var jwtSettings = mConfiguration.GetSection( "JWTSettings" );
            var credentials = GetSigningCredentials( jwtSettings );
            var expiration = DateTimeProvider.Instance.CurrentDateTime.AddMinutes(
                    Convert.ToDouble( jwtSettings["expiryInMinutes"]));
            var token = new JwtSecurityToken( issuer: jwtSettings["validIssuer"], audience: null, claims: claims, 
                                              expires: expiration, signingCredentials: credentials );

            return new LoginUserResponse( new JwtSecurityTokenHandler().WriteToken( token ), expiration );
        }

        private SigningCredentials GetSigningCredentials( IConfigurationSection jwtSettings ) { 
            var key = Encoding.UTF8.GetBytes( jwtSettings["securityKey"] ?? String.Empty ); 
            var secret = new SymmetricSecurityKey( key ); 
            
            return new SigningCredentials( secret, SecurityAlgorithms.HmacSha256 ); 
        }

        private async Task<List<Claim>> BuildUserClaims( IdentityUser user ) {
            var claims = new List<Claim> {
                new( ClaimTypes.Name, user.UserName ?? String.Empty ),
                new( "entityId", user.Id ),
                new( ClaimTypes.Email, user.Email ?? String.Empty )
            };

            var dbClaims = await mUserManager.GetClaimsAsync( user );

            claims.AddRange( dbClaims );

            var dbRoles = await mUserManager.GetRolesAsync( user );

            claims.AddRange( dbRoles.Select( r => new Claim( ClaimValues.ClaimRole, r )));

            return claims;
        }
    }
}
