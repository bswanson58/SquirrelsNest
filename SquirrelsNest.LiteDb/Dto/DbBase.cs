using LiteDB;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbBase {
        [BsonId]
        public  ObjectId    Id { get; }

        protected DbBase( ObjectId id ) {
            Id = id;
        }
    }
}
