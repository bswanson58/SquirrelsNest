using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Database {
    public class DatabaseInitializer {
        private readonly IUserProvider                  mUserProvider;
        private readonly UserManager<IdentityUser>      mUserManager;
        private readonly IConfiguration                 mConfiguration;
        private readonly ServiceDbContext               mContext;

        public DatabaseInitializer( UserManager<IdentityUser> userManager, IConfiguration configuration, 
                                    ServiceDbContext context, IUserProvider userProvider ) {
            mUserManager = userManager;
            mConfiguration = configuration;
            mUserProvider = userProvider;
            mContext = context;
        }

        public async Task<Either<Error, SnUser>> InitializeDatabase() {
            var haveAdmin = false;

            foreach( var user in mContext.Users ) {
                var claims = await mUserManager.GetClaimsAsync( user );

                haveAdmin = claims.Any( claim => claim.Type.Equals( ClaimValues.ClaimRole ) && claim.Value.Equals( ClaimValues.ClaimRoleAdmin ));

                if( haveAdmin ) {
                    break;
                }
            }

            if(!haveAdmin ) {
                return await CreateAdminUser();
            }

            return SnUser.Default;
        }

        private async Task<Either<Error, SnUser>> CreateAdminUser() {
            var user = new IdentityUser { UserName = mConfiguration["DefaultAdmin:Name"], Email = mConfiguration["DefaultAdmin:Email"] };
            var result = await mUserManager.CreateAsync( user, mConfiguration["DefaultAdmin:Password"] );

            if( result.Succeeded ) {
                result = await mUserManager.AddClaimAsync( user, new Claim( ClaimValues.ClaimRole, ClaimValues.ClaimRoleAdmin ));

                // all users have the user role.
                if( result.Succeeded ) {
                    result = await mUserManager.AddClaimAsync( user, new Claim( ClaimValues.ClaimRole, ClaimValues.ClaimRoleUser ));
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
