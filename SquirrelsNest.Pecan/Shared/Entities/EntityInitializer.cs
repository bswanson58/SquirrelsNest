namespace SquirrelsNest.Pecan.Shared.Entities {
    public interface IEntityInitializer {
        void    InitializeEntity( EntityBase forBase );
        void    InitializeEntity( EntityBase forBase, string eId );
    }

    internal static class EntityInitializer {
        private static IEntityInitializer ? mEntityInitializer;

        public  static IEntityInitializer   Instance => mEntityInitializer ?? CreateInitializer();

        public static void SetInitializer( IEntityInitializer initializer ) {
            mEntityInitializer = initializer;
        }

        private static IEntityInitializer CreateInitializer() {
            mEntityInitializer = new RuntimeEntityInitializer();

            return mEntityInitializer;
        }
    }

    internal class RuntimeEntityInitializer : IEntityInitializer {
        public void InitializeEntity( EntityBase entityBase ) {
            entityBase.EntityId = EntityId.CreateNew();
        }

        public void InitializeEntity( EntityBase entityBase, string entityId ) {
            entityBase.EntityId = EntityId.CreateIdOrThrow( entityId );
        }
    }
}
