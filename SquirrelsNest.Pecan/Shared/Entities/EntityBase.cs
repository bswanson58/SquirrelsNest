namespace SquirrelsNest.Pecan.Shared.Entities {
    public class EntityBase {
        public string   DbId { get; }
        public string   EntityId { get; internal set; }

        private EntityBase() {
            DbId = string.Empty;
            EntityId = string.Empty;
        }

        protected EntityBase( string dbId ) : this() {
            DbId = dbId;
        }

        protected EntityBase( string entityId, string dbId ) : this() {
            DbId = dbId;
            EntityId = entityId;
        }
    }
}
