using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;

namespace SquirrelsNest.Core.Database {
    internal class UserDataProvider : IUserDataProvider {
        private readonly IDbUserDataProvider    mDataProvider;

        public UserDataProvider( IDbUserDataProvider dataProvider ) {
            mDataProvider = dataProvider;
        }

        public async Task<Either<Error, SnUserData>> SaveData( SnUser user, SnUserData data ) {
            var existingData = await mDataProvider.GetData( user ).ConfigureAwait( false );
            var deletedData = await existingData.BindAsync( async list => {
                foreach ( var d in list ) {
                    if( d.DataType.Equals( data.DataType )) {
                        var result = await mDataProvider.DeleteData( d ).ConfigureAwait( false );
                        if( result.IsLeft ) {
                            return result;
                        }
                    }
                }

                return Unit.Default;
            });

            return await deletedData.BindAsync( async _ => await mDataProvider.AddData( data ).ConfigureAwait( false ));
        }

        public Task<Either<Error, IEnumerable<SnUserData>>> GetData( SnUser forUser ) => mDataProvider.GetData( forUser );
        public Task<Either<Error, SnUserData>> LoadData( SnUser forUser, UserDataType ofType ) => mDataProvider.GetData( forUser, ofType );

        public void Dispose() {
            mDataProvider.Dispose();
        }
    }
}
