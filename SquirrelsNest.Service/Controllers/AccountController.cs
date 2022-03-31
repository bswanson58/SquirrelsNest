using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SquirrelsNest.Service.Controllers.Dto;
using SquirrelsNest.Service.Database;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Controllers {
    [ApiController]
    [Route( "/account" )]
    public class AccountsController : ControllerBase {
        private readonly UserManager<IdentityUser>      mUserManager;
        private readonly SignInManager<IdentityUser>    mSignInManager;
        private readonly IConfiguration                 mConfiguration;
        private readonly ServiceDbContext               mContext;

        public AccountsController( UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
                                   IConfiguration configuration, ServiceDbContext context ) {
            mUserManager = userManager;
            mSignInManager = signInManager;
            mConfiguration = configuration;
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
                var role = mContext.Users.Length() == 1 ? "admin" : "user";

                result = await mUserManager.AddClaimAsync( user, new Claim( "role", role ));

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
            var claims = new List<Claim>() {
                new Claim( "email", userCredentials.Email )
            };

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
    }
}
