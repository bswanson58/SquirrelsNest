using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Controllers.Dto;
using SquirrelsNest.Service.Database;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Controllers {
    [ApiController]
    [Route( "/account" )]
    public class AccountsController : ControllerBase {
        private readonly IUserProvider                  mUserProvider;
        private readonly UserManager<IdentityUser>      mUserManager;
        private readonly SignInManager<IdentityUser>    mSignInManager;
        private readonly IConfiguration                 mConfiguration;
        private readonly ServiceDbContext               mContext;

        public AccountsController( UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
                                   IConfiguration configuration, ServiceDbContext context, IUserProvider userProvider ) {
            mUserManager = userManager;
            mSignInManager = signInManager;
            mConfiguration = configuration;
            mUserProvider = userProvider;
            mContext = context;
        }

        [HttpGet( "listUsers" )]
        [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin" )]
        public async Task<ActionResult<List<DbUser>>> GetListUsers( [FromQuery] PageInfo pageInfo ) {
            var queryable = mContext.Users.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader( queryable );
            var users = await queryable.OrderBy(x => x.Email).Paginate( pageInfo ).ToListAsync();

            return users.Select( u => new DbUser( u ) ).ToList();
        }

        [HttpPost( "makeAdmin" )]
        [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin" )]
        public async Task<ActionResult> MakeAdmin( [FromBody] string userId ) {
            var user = await mUserManager.FindByIdAsync(userId);
            await mUserManager.AddClaimAsync( user, new Claim( "role", "admin" ) );

            return NoContent();
        }

        [HttpPost( "removeAdmin" )]
        [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin" )]
        public async Task<ActionResult> RemoveAdmin( [FromBody] string userId ) {
            var user = await mUserManager.FindByIdAsync(userId);
            await mUserManager.RemoveClaimAsync( user, new Claim( "role", "admin" ) );

            return NoContent();
        }

        [HttpPost( "create" )]
        public async Task<ActionResult<AuthenticationResponse>> Create( [FromBody] UserCredentials userCredentials ) {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await mUserManager.CreateAsync(user, userCredentials.Password);

            if( result.Succeeded ) {
                // make the first user to be created an admin
                if( mContext.Users.Length() == 1 ) {
                    result = await mUserManager.AddClaimAsync( user, new Claim( "role", "admin" ));
                }

                // all users have the user role.
                if( result.Succeeded ) {
                    result = await mUserManager.AddClaimAsync( user, new Claim( "role", "user" ));
                }

                if( result.Succeeded ) {
                    var dbUser = await InsureUserExists( userCredentials );
                    var retValue = dbUser.Match( 
                        u => new ObjectResult( u ), 
                        e => Problem( title: "Error Creating SnUser", detail: e.Message ));

                    if( dbUser.IsLeft ) {
                        return retValue;
                    }
                }

                if( result.Succeeded ) {
                    return await BuildToken( userCredentials );
                }
            }

            return BadRequest( result.Errors );
        }

        [HttpPost( "login" )]
        public async Task<ActionResult<AuthenticationResponse>> Login( [FromBody] UserCredentials userCredentials ) {
            var result = await mSignInManager.PasswordSignInAsync( userCredentials.Email, userCredentials.Password,
                                                                   isPersistent: false, lockoutOnFailure: false);
            if( result.Succeeded ) {
                return await BuildToken( userCredentials );
            }

            return BadRequest( "Incorrect Login" );
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
                new Claim( "email", email )
            };

            var user = await mUserProvider.GetUser( email );

            user.Do( u => {
                claims.Add( new Claim( "name", u.Name ));
                claims.Add( new Claim( "entityId", u.EntityId ));
            });

            return claims;
        }

        private async Task<Either<Error, SnUser>> InsureUserExists( UserCredentials user ) {
            var dbUser = await mUserProvider.GetUser( user.Email );

            if( dbUser.IsLeft ) {
                var newUser = new SnUser( user.Email, user.Email ).With( displayName: user.Name );

                return await mUserProvider.AddUser( newUser );
            }

            return dbUser;
        }
    }
}
