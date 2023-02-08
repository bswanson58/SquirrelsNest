using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class UserDataProvider : EntityProvider<SnUserData, DbUserData>, IDbUserDataProvider {
        public UserDataProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnUserData ConvertTo( DbUserData data ) => data.ToEntity();
        protected override DbUserData ConvertFrom( SnUserData data ) => DbUserData.From( data );

        public Task<Either<Error, SnUserData>> AddData( SnUserData data ) => AddEntity( data );
        public Task<Either<Error, Unit>> DeleteData( SnUserData data ) => DeleteEntity( data );
        public Task<Either<Error, Unit>> UpdateData( SnUserData data ) => UpdateEntity( data );

        public Task<Either<Error, IEnumerable<SnUserData>>> GetData( SnUser forUser ) => 
            GetEntities( a => a.UserId.Equals( forUser.EntityId ));

        public async Task<Either<Error, SnUserData>> GetData( SnUser forUser, UserDataType ofType ) {
            var data = await GetEntities( a => a.UserId.Equals( forUser.EntityId ) && a.DataType.Equals( ofType ));

            return data.Map( list => list.FirstOrDefault( SnUserData.Default ));
        }
    }
}
