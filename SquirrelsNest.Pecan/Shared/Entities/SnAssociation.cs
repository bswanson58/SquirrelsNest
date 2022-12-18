using System;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnAssociation : EntityBase {
        public  EntityIdentifier    OwnerId { get; }
        public  EntityIdentifier    AssociationId { get; }

        public SnAssociation( string entityId, string ownerId, string associationId ) :
            base( entityId ){
            OwnerId = EntityIdentifier.CreateIdOrThrow( ownerId );
            AssociationId = EntityIdentifier.CreateIdOrThrow( associationId );
        }

        public SnAssociation( EntityIdentifier ownerId, EntityIdentifier associationId ) :
            base( String.Empty ) {
            OwnerId = ownerId;
            AssociationId = associationId;
        }

        private static SnAssociation ? mDefaultUser;

        public static SnAssociation Default =>
            mDefaultUser ??= new SnAssociation( EntityIdentifier.Default, EntityIdentifier.Default, EntityIdentifier.Default );
    }
}
