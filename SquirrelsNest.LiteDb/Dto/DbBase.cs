using LiteDB;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbBase {
        public  ObjectId    Id { get; }

        protected DbBase( ObjectId id ) {
            Id = id;
        }
    }
}
