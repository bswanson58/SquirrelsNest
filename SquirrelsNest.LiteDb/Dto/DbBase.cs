using LiteDB;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbBase {
        [BsonId]
        public  ObjectId    Id { get; set; }
        public  string      EntityId { get; set; }

        protected DbBase() {
            Id = ObjectId.Empty;
            EntityId = String.Empty;
        }
    }
}
