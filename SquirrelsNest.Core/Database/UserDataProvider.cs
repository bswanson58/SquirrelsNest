using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;

namespace SquirrelsNest.Core.Database {
    internal class UserDataProvider : IUserDataProvider {
        private readonly IDbUserDataProvider    mDataProvider;
        private readonly IDbUserProvider        mUserProvider;

        public UserDataProvider( IDbUserDataProvider dataProvider, IDbUserProvider userProvider ) {
            mDataProvider = dataProvider;
            mUserProvider = userProvider;
        }

        public async Task<Either<Error, Unit>> SaveData( SnUserData data ) {
            var user = await mUserProvider.GetUser( data.UserId ).ConfigureAwait( false );
            var existingData = await user.BindAsync( async u => await mDataProvider.GetData( u, data.DataType )).ConfigureAwait( false );
            var deletedData = await existingData.BindAsync( async d => {
                if(!d.EntityId.Equals( SnUserData.Default.EntityId )) {
                    return await mDataProvider.DeleteData( d ).ConfigureAwait( false );
                }

                return Unit.Default;
            });

            return await deletedData.BindAsync( async _ => {
                    return ( await mDataProvider.AddData( data ).ConfigureAwait( false ))
                        .Map( _ => Unit.Default );
                });
        }

        public Task<Either<Error, IEnumerable<SnUserData>>> GetData( SnUser forUser ) => mDataProvider.GetData( forUser );
        public Task<Either<Error, SnUserData>> LoadData( SnUser forUser, UserDataType ofType ) => mDataProvider.GetData( forUser, ofType );

        public void Dispose() {
            mDataProvider.Dispose();
        }
    }
}
