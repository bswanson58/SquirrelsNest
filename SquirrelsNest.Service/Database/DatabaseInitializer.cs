using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Database {
    public class IdentityDatabaseInitializer {
        private readonly IUserProvider                  mUserProvider;
        private readonly IConfiguration                 mConfiguration;
        private readonly ServiceDbContext               mIdentityContext;

        public IdentityDatabaseInitializer( IConfiguration configuration,
                                            ServiceDbContext identityContext, IUserProvider userProvider ) {
            mConfiguration = configuration;
            mUserProvider = userProvider;
            mIdentityContext = identityContext;
        }

        public async Task<Either<Error, SnUser>> InitializeDatabase( UserManager<IdentityUser> userManager ) {
            await mIdentityContext.Database.MigrateAsync();

            var haveAdmin = false;

            foreach( var user in mIdentityContext.Users ) {
                var claims = await userManager.GetClaimsAsync( user );

                haveAdmin = claims.Any( claim => claim.Type.Equals( ClaimValues.ClaimRole ) && claim.Value.Equals( ClaimValues.ClaimRoleAdmin ));

                if( haveAdmin ) {
                    break;
                }
            }

            if(!haveAdmin ) {
                return await CreateAdminUser( userManager );
            }

            return SnUser.Default;
        }

        private async Task<Either<Error, SnUser>> CreateAdminUser( UserManager<IdentityUser> userManager ) {
            var user = new IdentityUser { UserName = mConfiguration["DefaultAdmin:Email"], Email = mConfiguration["DefaultAdmin:Email"] };
            var result = await userManager.CreateAsync( user, mConfiguration["DefaultAdmin:Password"] );

            if( result.Succeeded ) {
                result = await userManager.AddClaimAsync( user, new Claim( ClaimValues.ClaimRole, ClaimValues.ClaimRoleAdmin ));

                // all users have the user role.
                if( result.Succeeded ) {
                    result = await userManager.AddClaimAsync( user, new Claim( ClaimValues.ClaimRole, ClaimValues.ClaimRoleUser ));
                }

                if( result.Succeeded ) {
                    return await InsureUserExists( user );
                }

                return Error.New( 0, "UserManager.AddClaim failed." );
            }

            return Error.New( 0, "UserManager.Create failed." );
        }

        private async Task<Either<Error, SnUser>> InsureUserExists( IdentityUser user ) {
            var dbUser = await mUserProvider.GetUser( user.Email );

            if( dbUser.IsLeft ) {
                var newUser = new SnUser( user.Email, user.Email ).With( displayName: user.NormalizedUserName );

                return await mUserProvider.AddUser( newUser );
            }

            return dbUser;
        }
    }
}
