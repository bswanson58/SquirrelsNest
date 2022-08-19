using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Users {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQuery {
        private readonly IUserProvider  mUserProvider;

        public UserQuery( IUserProvider userProvider ) {
            mUserProvider = userProvider;
        }

        [UseOffsetPaging(MaxPageSize = 10, IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<IEnumerable<ClUser>> UserList() {
            var users = await mUserProvider.GetUsers();
            var clUsers = users.Map( list => list.Map( u => u.ToCl()));

            return clUsers.Match( list => list, _ => new List<ClUser>());
        }
    }
}
