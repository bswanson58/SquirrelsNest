using System;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbAssociation : DbEntityBase<DbAssociation> {
        public  string  OwnerId { get; set; }
        public  string  AssociationId { get; set; }

        public DbAssociation() {
            OwnerId = String.Empty;
            AssociationId = String.Empty;
        }

        public DbAssociation( SnAssociation association ) :
            base( association.EntityId ) {
            OwnerId = association.OwnerId;
            AssociationId = association.AssociationId;
        }
        
        public static DbAssociation From( SnAssociation association ) => new( association );

        public SnAssociation ToEntity() => new( EntityId, OwnerId, AssociationId );

        public override void UpdateFrom( DbAssociation from ) {
            OwnerId = from.OwnerId;
            AssociationId = from.AssociationId;
        }
    }
}
