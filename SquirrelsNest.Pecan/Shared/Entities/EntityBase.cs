namespace SquirrelsNest.Pecan.Shared.Entities {
    public class EntityBase {
        public EntityId EntityId { get; internal set; }

        private EntityBase() {
            EntityId = EntityId.CreateNew();
        }

        protected EntityBase( string entityId ) :
            this() {
            EntityId = EntityId.CreateIdOrThrow( entityId );
        }
    }
}
