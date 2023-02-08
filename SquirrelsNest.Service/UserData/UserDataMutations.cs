using System.Threading.Tasks;
using HotChocolate;
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

        public UserDataMutations( IUserProvider userProvider,
                                  IHttpContextAccessor contextAccessor, IApplicationLog log ) :
            base( userProvider, contextAccessor, log ) {
        }

        // ReSharper disable once UnusedMember.Global
        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<UserDataPayload> SaveUserData([Service(ServiceKind.Synchronized)]IUserDataProvider userDataProvider, 
                                                        UserDataInput dataInput ) {
            var user = await GetUser();
            var result = await user.BindAsync( async u => {
                var userData = new SnUserData( u.EntityId, dataInput.DataType, dataInput.Data );

                return await userDataProvider.SaveData( u, userData );
            });

            return result.Match( d => new UserDataPayload( d.ToCl()), e => new UserDataPayload( e ));
        }
    }
}
