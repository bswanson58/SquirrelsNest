namespace SquirrelsNest.Pecan.Shared.Entities {
    public class EntityBase {
        public  string  EntityId { get; }

        protected EntityBase() {
            EntityId = EntityIdentifier.CreateNew();
        }

        protected EntityBase( string entityId ) :
            this() {
            EntityId = EntityIdentifier.CreateIdOrThrow( entityId );
        }
    }
}
