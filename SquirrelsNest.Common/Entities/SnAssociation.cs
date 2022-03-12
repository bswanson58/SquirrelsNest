using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Entities {
    public class SnAssociation : EntityBase {
        public  EntityId    OwnerId { get; }
        public  EntityId    AssociationId { get; }

        public SnAssociation( string entityId, string dbId, string ownerId, string associationId ) :
            base( entityId, dbId ){
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
            mDefaultUser ??= new SnAssociation( EntityId.Default, String.Empty, EntityId.Default, EntityId.Default );
    }
}
