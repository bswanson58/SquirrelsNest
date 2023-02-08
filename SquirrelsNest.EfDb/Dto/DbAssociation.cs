using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.EfDb.Dto {
    internal class DbAssociation : DbBase {
        public  string  OwnerId { get; set; }
        public  string  AssociationId { get; set; }

        protected DbAssociation() {
            OwnerId = String.Empty;
            AssociationId = String.Empty;
        }

        public static DbAssociation From( SnAssociation association ) {
            return new DbAssociation {
                EntityId = association.EntityId,
                Id = String.IsNullOrWhiteSpace( association.DbId ) ? DbIdDefault() : DbIdCreate( association.DbId ),
                OwnerId = association.OwnerId,
                AssociationId = association.AssociationId
            };
        }

        public SnAssociation ToEntity() {
            return new SnAssociation( EntityId, Id.ToString(),
                                      Common.Values.EntityId.CreateIdOrThrow( OwnerId ),
                                      Common.Values.EntityId.CreateIdOrThrow( AssociationId ));
        }
    }
}
