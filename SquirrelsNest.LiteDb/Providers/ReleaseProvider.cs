using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ReleaseProvider : BaseProvider<SnRelease, DbRelease> {
        private static SnRelease ConvertTo( DbRelease release ) => release.ToEntity();
        private static DbRelease ConvertFrom( SnRelease release ) => DbRelease.From( release );

        public ReleaseProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.ReleaseCollection, ConvertFrom, ConvertTo ) {
        }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbRelease>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnRelease> AddRelease( SnRelease release ) => Add( release );
        public Either<Error, Unit> UpdateRelease( SnRelease release ) => Update( release );
        public Either<Error, Unit> DeleteRelease( SnRelease release ) => Delete( release );
        public Either<Error, SnRelease> GetRelease( EntityId releaseId ) => Get( releaseId );
        public Either<Error, IEnumerable<SnRelease>> GetReleases() => GetEnumerable();
    }
}
