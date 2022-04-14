using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Controllers.Dto;

namespace SquirrelsNest.Service.Users {
    [ExtendObjectType(OperationTypeNames.Query)]
    public class Authentication {
        private readonly IUserProvider                  mUserProvider;
        private readonly UserManager<IdentityUser>      mUserManager;
        private readonly SignInManager<IdentityUser>    mSignInManager;
        private readonly IConfiguration                 mConfiguration;

        public Authentication( IUserProvider userProvider, UserManager<IdentityUser> userManager, 
                               SignInManager<IdentityUser> signInManager, IConfiguration configuration ) {
            mUserProvider = userProvider;
            mUserManager = userManager;
            mSignInManager = signInManager;
            mConfiguration = configuration;
        }

        private async Task<AuthenticationResponse> BuildToken( UserCredentials userCredentials ) {
            var claims = await BuildUserClaims( userCredentials.Email );
            var user = await mUserManager.FindByNameAsync( userCredentials.Email );
            var dbClaims = await mUserManager.GetClaimsAsync( user );

            claims.AddRange( dbClaims );

            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( mConfiguration["JwtKey"]));
            var credentials = new SigningCredentials( key, SecurityAlgorithms.HmacSha256 );

            var expiration = DateTime.UtcNow.AddYears( 1 );

            var token = new JwtSecurityToken( issuer: null, audience: null, claims: claims, expires: expiration, 
                signingCredentials: credentials );

            return new AuthenticationResponse() {
                Token = new JwtSecurityTokenHandler().WriteToken( token ),
                Expiration = expiration
            };
        }

        private async Task<List<Claim>> BuildUserClaims( string email ) {
            var claims = new List<Claim>() {
                new ( "email", email )
            };

            var user = await mUserProvider.GetUser( email );

            user.Do( u => {
                claims.Add( new Claim( "name", u.Name ));
                claims.Add( new Claim( "entityId", u.EntityId ));
            });

            return claims;
        }

        // ReSharper disable once UnusedMember.Global
        public async Task<AuthenticationResponse> Login( UserCredentials userCredentials ) {
            var result = await mSignInManager.PasswordSignInAsync( userCredentials.Email, userCredentials.Password,
                                                                   isPersistent: false, lockoutOnFailure: false);

            if( result.Succeeded ) {
                return await BuildToken( userCredentials );
            }

            return new AuthenticationResponse();
        }
    }
}
