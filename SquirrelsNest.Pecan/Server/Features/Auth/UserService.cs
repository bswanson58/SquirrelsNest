using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SquirrelsNest.Pecan.Server.Database;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Auth {
    public interface IUserService {
        Task<SnUser ?> CreateUser( string email, string displayName, string password = "" );
        Task<bool>      UpdatePassword( SnUser user, string currentPassword, string newPassword );
    }

    public class UserService : IUserService {
        private readonly IDbContext             mContext;
        private readonly IUserProvider          mUserProvider;
        private readonly UserManager<DbUser>    mUserManager;

        public UserService( UserManager<DbUser> userManager, IDbContext context, IUserProvider userProvider ) {
            mUserManager = userManager;
            mContext = context;
            mUserProvider = userProvider;
        }

        public async Task<SnUser ?> CreateUser( string email, string displayName, string password = "" ) {
            var user = new DbUser { UserName = email, Email = email };
            var firstUser = !mContext.Users.Any();
            var result = await mUserManager.CreateAsync( user );

            if(( result.Succeeded ) &&
               (!String.IsNullOrWhiteSpace( password ))) {
                result = await mUserManager.AddPasswordAsync( user, password );
            }

            if( result.Succeeded ) {
                result = await mUserManager.AddClaimAsync( user, new Claim( ClaimTypes.GivenName, displayName ));
            }

            if( result.Succeeded ) {
                // make the first user to be created an admin
                if( firstUser ) {
                    result = await mUserManager.AddToRoleAsync( user, ClaimValues.ClaimRoleAdmin );
                }

                // all users have the user role.
                if( result.Succeeded ) {
                    result = await mUserManager.AddToRoleAsync( user, ClaimValues.ClaimRoleUser );
                }

                if( result.Succeeded ) {
                    return await mUserProvider.GetById( user.Id );
                }
            }

            if(!result.Succeeded ) {
                throw new ApplicationException( String.Join( Environment.NewLine, result.Errors.Select( e => e.Description )));
            }

            return null;
        }

        public async Task<bool> UpdatePassword( SnUser user, string currentPassword, string newPassword ) {
            var dbUser = await mUserManager.FindByIdAsync( user.EntityId );

            if( dbUser != null ) {
                if( await mUserManager.HasPasswordAsync( dbUser )) {
                    var changeResult = await mUserManager.ChangePasswordAsync( dbUser, currentPassword, newPassword );

                    if(!changeResult.Succeeded ) {
                        throw new ApplicationException( 
                            String.Join( Environment.NewLine, changeResult.Errors.Select( e => e.Description )));
                    }

                    return true;
                }

                var addResult = await mUserManager.AddPasswordAsync( dbUser, newPassword );

                if(!addResult.Succeeded ) {
                    throw new ApplicationException( 
                        String.Join( Environment.NewLine, addResult.Errors.Select( e => e.Description )));
                }

                return true;
            }

            throw new ApplicationException( "User could not be located" );
        }
    }
}
