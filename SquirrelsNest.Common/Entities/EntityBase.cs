using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Entities {
    public class EntityBase {
        public  string      DbId { get; }
        public  EntityId    EntityId { get; internal set; }

        private EntityBase() {
            DbId = String.Empty;
            EntityId = EntityId.Default;
        }

        protected EntityBase( string dbId ) : this() {
            DbId = dbId;

            EntityInitializer.Instance.InitializeEntity( this );
        }

        protected EntityBase( string entityId, string dbId ) : this() {
            DbId = dbId;

            EntityInitializer.Instance.InitializeEntity( this, entityId );
        }
    }
}
