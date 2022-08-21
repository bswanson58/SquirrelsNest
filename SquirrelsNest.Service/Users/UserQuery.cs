using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Identity;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Users {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQuery {
        private readonly IUserProvider              mUserProvider;
        private readonly UserManager<IdentityUser>  mUserManager;
        private readonly IApplicationLog            mLog;

        public UserQuery( IUserProvider userProvider, UserManager<IdentityUser> userManager, IApplicationLog log ) {
            mUserProvider = userProvider;
            mUserManager = userManager;
            mLog = log;
        }

        private async Task<IEnumerable<ClUser>> AddUserClaims( IEnumerable<SnUser> users ) {
            var retValue = new List<ClUser>();

            try {
                foreach( var u in users ) {
                    var user = await mUserManager.FindByNameAsync( u.Email );

                    if( user != null ) {
                        var dbClaims = await mUserManager.GetClaimsAsync( user );

                        retValue.Add( u.ToCl().With( dbClaims ));
                    }
                }
            }
            catch( Exception ex ) {
                mLog.LogException( "AddUserClaims", ex );
            }

            return retValue;
        }

        [UseOffsetPaging(MaxPageSize = 10, IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<IEnumerable<ClUser>> UserList() {
            var users = await mUserProvider.GetUsers();
            var clUsers = await users.MapAsync( AddUserClaims );

            return clUsers.Match( list => list, _ => new List<ClUser>());
        }
    }
}
