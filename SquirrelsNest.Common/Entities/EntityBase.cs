namespace SquirrelsNest.Common.Entities {
    public class EntityBase {
        public  string  EntityId { get; }

        protected EntityBase( string entityId ) {
            EntityId = entityId;
        }
    }
}
