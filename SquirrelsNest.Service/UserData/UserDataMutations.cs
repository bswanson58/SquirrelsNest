using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Dto.Mutations;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.UserData {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class UserDataMutations : BaseGraphProvider {
        private readonly IUserDataProvider  mUserDataProvider;

        public UserDataMutations( IUserDataProvider userDataProvider, IUserProvider userProvider,
                                  IHttpContextAccessor contextAccessor, IApplicationLog log ) :
            base( userProvider, contextAccessor, log ) {
            mUserDataProvider = userDataProvider;
        }

        // ReSharper disable once UnusedMember.Global
        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<UserDataPayload> SaveUserData( UserDataInput dataInput ) {
            var user = await GetUser();
            var data = user.Map( u => new SnUserData( u.EntityId, dataInput.DataType, dataInput.Data ));
            var result = await data.BindAsync( d => mUserDataProvider.SaveData( d ));

            return result.Match( d => new UserDataPayload( d.ToCl()), e => new UserDataPayload( e ));
        }
    }
}
