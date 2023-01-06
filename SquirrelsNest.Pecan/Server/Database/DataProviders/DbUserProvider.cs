using System;
using System.Collections.Generic;
using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IUserProvider {
        Task<IEnumerable<SnUser>>   GetAll();
        ValueTask<SnUser ?>         GetById( string id );
        ValueTask<SnUser ?>         GetFromContext( HttpContext context );
    }

    public class SnUserProvider : IUserProvider {
        private readonly PecanDbContext         mContext;
        private readonly UserManager<DbUser>    mUserManager;

        public SnUserProvider( PecanDbContext context, UserManager<DbUser> userManager ) {
            mContext = context;
            mUserManager = userManager;
        }

        public async Task<IEnumerable<SnUser>> GetAll() {
            var retValue = new List<SnUser>();
            var users = await mContext.Users.ToListAsync();

            foreach( var user in users ) {
                retValue.Add( await ConvertTo( user ));
            }

            return retValue;
        }

        public async ValueTask<SnUser ?> GetById( string id ) {
            if(( String.IsNullOrWhiteSpace( id )) ||
               ( id.Equals( SnUser.Default.EntityId ))) {
                return null;
            }

            var user = await mContext.Users.FindAsync( id );

            return user != null ? await ConvertTo( user ) : null;
        }

        public ValueTask<SnUser ?> GetFromContext( HttpContext context ) {
            var userId = context.User.Claims
                .FirstOrDefault( c => c.Type.Equals( ClaimValues.ClaimEntityId ))?.Value ?? String.Empty;

            if(!String.IsNullOrWhiteSpace( userId )) {
                return GetById( userId );
            }

            return default;
        }

        private async Task<SnUser> ConvertTo( DbUser user ) {
            var claims = await mUserManager.GetClaimsAsync( user );
            var nameClaim = claims.FirstOrDefault( c => c.Type.Equals( ClaimTypes.GivenName ));
            var name = nameClaim != null ? nameClaim.Value : String.Empty;

            return new SnUser( user.Id, user.UserName ?? String.Empty, name, user.Email ?? String.Empty );
        }
    }
}
