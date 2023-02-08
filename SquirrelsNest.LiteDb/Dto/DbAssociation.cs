using LiteDB;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbAssociation : DbBase {
        public  string      OwnerId { get; set; }
        public  string      AssociationId { get; set; }

        protected DbAssociation() {
            OwnerId = Common.Values.EntityId.Default;
            AssociationId = Common.Values.EntityId.Default;
        }

        public static DbAssociation From( SnAssociation association ) {
            return new DbAssociation() {
                Id = String.IsNullOrWhiteSpace( association.DbId ) ? ObjectId.NewObjectId() : new ObjectId( association.DbId ),
                EntityId = association.EntityId,
                OwnerId = association.OwnerId,
                AssociationId = association.AssociationId,
            };
        }

        public SnAssociation ToEntity() {
            return new SnAssociation( EntityId, Id.ToString(), OwnerId, AssociationId );
        }
    }
}
