using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class AssociationProvider : BaseProvider<SnAssociation, DbAssociation> {
        private static SnAssociation ConvertTo( DbAssociation association ) => association.ToEntity();
        private static DbAssociation ConvertFrom( SnAssociation association ) => DbAssociation.From( association );

        public AssociationProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.AssociationCollection, ConvertFrom, ConvertTo ) {
        }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbAssociation>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnAssociation> AddAssociation( SnAssociation association ) => Add( association );
        public Either<Error, Unit> DeleteAssociation( SnAssociation association ) => Delete( association );
        public Either<Error, SnAssociation> GetAssociation( EntityId componentId ) => Get( componentId );

        public Either<Error, IEnumerable<SnAssociation>> GetAssociations( SnUser forUser ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbAssociation.OwnerId ), forUser.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }

        public Either<Error, IEnumerable<SnAssociation>> GetAssociations( EntityId associationId ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbAssociation.AssociationId ), associationId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}
