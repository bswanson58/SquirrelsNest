using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ComponentProvider : BaseProvider<SnComponent, DbComponent> {
        private static SnComponent ConvertTo( DbComponent component ) => component.ToEntity();
        private static DbComponent ConvertFrom( SnComponent component ) => DbComponent.From( component );

        public ComponentProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.ComponentCollection, ConvertFrom, ConvertTo ) {
        }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbComponent>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnComponent> AddComponent( SnComponent component ) => Add( component );
        public Either<Error, Unit> UpdateComponent( SnComponent component ) => Update( component );
        public Either<Error, Unit> DeleteComponent( SnComponent component ) => Delete( component );
        public Either<Error, SnComponent> GetComponent( EntityId componentId ) => Get( componentId );
        public Either<Error, IEnumerable<SnComponent>> GetComponents() => GetEnumerable();

        public Either<Error, IEnumerable<SnComponent>> GetComponents( SnProject forProject ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbComponent.ProjectId ), forProject.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}