using System;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnAssociation : EntityBase {
        public  EntityId    OwnerId { get; }
        public  EntityId    AssociationId { get; }

        public SnAssociation( string entityId, string ownerId, string associationId ) :
            base( entityId ){
            OwnerId = EntityId.CreateIdOrThrow( ownerId );
            AssociationId = EntityId.CreateIdOrThrow( associationId );
        }

        public SnAssociation( EntityId ownerId, EntityId associationId ) :
            base( String.Empty ) {
            OwnerId = ownerId;
            AssociationId = associationId;
        }

        private static SnAssociation ? mDefaultUser;

        public static SnAssociation Default =>
            mDefaultUser ??= new SnAssociation( EntityId.Default, EntityId.Default, EntityId.Default );
    }
}
