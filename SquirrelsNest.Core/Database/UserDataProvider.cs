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
            var user = await mUserProvider.GetUser( data.UserId );

            if( user.IsLeft ) {
                return user.Map( _ => Unit.Default );
            }

            var existingData = await user.BindAsync( async u => await mDataProvider.GetData( u, data.DataType ));

            return await existingData.BindAsync( async d => {
                    if( d.EntityId.Equals( SnUserData.Default.EntityId )) {
                        var retValue = await mDataProvider.AddData( data );

                        return retValue.Map( _ => Unit.Default );
                    }

                    return await mDataProvider.UpdateData( data );
                });
        }

        public Task<Either<Error, IEnumerable<SnUserData>>> GetData( SnUser forUser ) => mDataProvider.GetData( forUser );
        public Task<Either<Error, SnUserData>> LoadData( SnUser forUser, UserDataType ofType ) => mDataProvider.GetData( forUser, ofType );

        public void Dispose() {
            mDataProvider.Dispose();
        }
    }
}
