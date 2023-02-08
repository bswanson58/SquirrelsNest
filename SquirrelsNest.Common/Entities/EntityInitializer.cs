using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Entities {
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
            if( EntityId.For( Guid.NewGuid().ToString()).Do( eId => entityBase.EntityId = eId ).IsNone ) {
                throw new ApplicationException( "EntityId could not be created" );
            }
        }

        public void InitializeEntity( EntityBase entityBase, string entityId ) {
            if( EntityId.For( entityId ).Do( eId => entityBase.EntityId = eId ).IsNone ) {
                throw new ApplicationException( "EntityId could not be created from string" );
            }
        }
    }
}
