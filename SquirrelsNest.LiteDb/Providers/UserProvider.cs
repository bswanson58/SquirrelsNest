using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class UserProvider : BaseProvider<SnUser, DbUser> {
        private static SnUser ConvertTo( DbUser user ) => user.ToEntity();
        private static DbUser ConvertFrom( SnUser user ) => DbUser.From( user );

        public UserProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.UserCollection, ConvertFrom, ConvertTo ) {
        }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbUser>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnUser> AddUser( SnUser user ) => Add( user );
        public Either<Error, Unit> UpdateUser( SnUser user ) => Update( user );
        public Either<Error, Unit> DeleteUser( SnUser user ) => Delete( user );
        public Either<Error, SnUser> GetUser( EntityId userId ) => Get( userId );
        public Either<Error, IEnumerable<SnUser>> GetUsers() => GetEnumerable();

    }
}
