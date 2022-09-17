using System.Text;
using System.Text.Json;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.Database {
    internal class UserData : IUserData {
        private readonly IUserDataProvider  mUserDataProvider;

        public UserData( IUserDataProvider userDataProvider ) {
            mUserDataProvider = userDataProvider;
        }

        public async Task<Either<Error, T>> Load<T>( SnUser user, UserDataType ofType ) where T : new() {
            var userData = await mUserDataProvider.LoadData( user, ofType );

            return userData.Bind( data => {
                if( String.IsNullOrWhiteSpace( data.Data )) {
                    return new T();
                }
                
                var stream = new MemoryStream( Encoding.UTF8.GetBytes( data.Data ));
                using( stream ) {
                    return Prelude.Try( () => JsonSerializer.Deserialize<T>( stream ) ?? new T()).ToEither( Error.New );
                }
            });
        }

        public async Task<Either<Error, T>> Save<T>( SnUser user, UserDataType ofType, T data ) {
            return await Prelude.Try( JsonSerializer.Serialize( data )).ToEither( Error.New )
                .BindAsync( async jsonData => {
                    var userData = new SnUserData( user.EntityId, ofType, jsonData );
                    var result = await mUserDataProvider.SaveData( userData );

                    return result.Map( _ => data );
                });
        }
    }
}
