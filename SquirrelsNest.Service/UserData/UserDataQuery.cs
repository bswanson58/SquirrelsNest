using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Support;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Service.Dto.Mutations;

namespace SquirrelsNest.Service.UserData {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserDataQuery : BaseGraphProvider {
        private readonly IUserDataProvider  mUserDataProvider;

        public UserDataQuery( IUserDataProvider userDataProvider, IUserProvider userProvider,
                              IHttpContextAccessor contextAccessor, IApplicationLog log ) :
            base( userProvider, contextAccessor, log ) {
            mUserDataProvider = userDataProvider;
        }

        // ReSharper disable once UnusedMember.Global
        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<UserDataPayload> GetUserData( UserDataType dataType ) {
            var user = await GetUser();
            var data = await user.BindAsync( u => mUserDataProvider.LoadData( u, dataType ));

            return data.Match( d => new UserDataPayload( d.ToCl()), e => new UserDataPayload( e ));
        }
    }
}
