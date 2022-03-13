using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class UserDataProvider : BaseProvider<SnUserData, DbUserData> {
        private static SnUserData ConvertTo( DbUserData data ) => data.ToEntity();
        private static DbUserData ConvertFrom( SnUserData data ) => DbUserData.From( data );

        public UserDataProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.UserDataCollection, ConvertFrom, ConvertTo ) {
        }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbUserData>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnUserData> AddData( SnUserData data ) => Add( data );
        public Either<Error, Unit> UpdateData( SnUserData data ) => Update( data );
        public Either<Error, Unit> DeleteData( SnUserData data ) => Delete( data );

        public Either<Error, SnUserData> GetData( SnUser forUser, UserDataType ofType ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbUserData.UserId ), forUser.EntityId.Value )))
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbUserData.DataType ), ofType.ToString())))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity())
                .Map( entityList => entityList.FirstOrDefault( SnUserData.Default ));
        }

        public Either<Error, IEnumerable<SnUserData>> GetData( SnUser forUser ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbUserData.UserId ), forUser.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}
