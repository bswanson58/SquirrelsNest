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

        public Either<Error, IEnumerable<SnRelease>> GetReleases( SnProject forProject ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbRelease.ProjectId ), forProject.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}
