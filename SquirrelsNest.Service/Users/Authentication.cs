using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Database;
using SquirrelsNest.Service.Dto.Mutations;

namespace SquirrelsNest.Service.Users {
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class Authentication {
        private readonly IUserProvider                  mUserProvider;
        private readonly IConfiguration                 mConfiguration;
        private readonly IdentityDatabaseInitializer    mDatabaseInitializer;

        public Authentication( IUserProvider userProvider, IConfiguration configuration, 
                               IdentityDatabaseInitializer databaseInitializer ) {
            mUserProvider = userProvider;
            mConfiguration = configuration;
            mDatabaseInitializer = databaseInitializer;
        }

        private LoginPayload BuildToken( IEnumerable<Claim> claims ) {
            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( mConfiguration["JwtKey"]));
            var credentials = new SigningCredentials( key, SecurityAlgorithms.HmacSha256 );

            var expiration = DateTime.UtcNow.AddYears( 1 );

            var token = new JwtSecurityToken( issuer: null, audience: null, claims: claims, expires: expiration, 
                signingCredentials: credentials );

            return new LoginPayload() {
                Token = new JwtSecurityTokenHandler().WriteToken( token ),
                Expiration = expiration
            };
        }

        private async Task<List<Claim>> BuildUserClaims( string email ) {
            var claims = new List<Claim>();
            var user = await mUserProvider.GetUser( email );

            user.Do( u => {
                claims.Add( new Claim( "name", u.Name ));
                claims.Add( new Claim( "entityId", u.EntityId ));
                claims.Add( new Claim( "email", u.Email ));
            });

            return claims;
        }

        // ReSharper disable once UnusedMember.Global
        public async Task<LoginPayload> Login([FromServices] UserManager<IdentityUser> userManager,
                                              [FromServices] SignInManager<IdentityUser> signInManager, 
                                              LoginInput loginInput ) {
//            var initResult = await mDatabaseInitializer.InitializeDatabase( userManager );
//            if( initResult.IsLeft ) {
//                return new LoginPayload( initResult.LeftToList().FirstOrDefault());
//            }

            var result = await signInManager.PasswordSignInAsync( loginInput.Email, loginInput.Password,
                                                                  isPersistent: false, lockoutOnFailure: false);

            if( result.Succeeded ) {
                var user = await userManager.FindByNameAsync( loginInput.Email );

                if( user != null ) {
                    var claims = await BuildUserClaims( loginInput.Email );
                    var dbClaims = await userManager.GetClaimsAsync( user );

                    claims.AddRange( dbClaims );

                    return BuildToken( claims );
                }
            }
            else {
                return new LoginPayload( result.ToString());
            }

            return new LoginPayload();
        }
    }
}
