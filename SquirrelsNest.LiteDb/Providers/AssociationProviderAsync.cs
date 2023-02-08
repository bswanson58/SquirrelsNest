using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class AssociationProviderAsync : AssociationProvider, IDbAssociationProvider {
        public AssociationProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnAssociation>> AddAssociation( SnAssociation association ) {
            return Task.Run(() => base.AddAssociation( association ));
        }

        public new Task<Either<Error, Unit>> DeleteAssociation( SnAssociation association ) {
            return Task.Run(() => base.DeleteAssociation( association ));
        }

        public new Task<Either<Error, SnAssociation>> GetAssociation( EntityId associationId ) {
            return Task.Run(() => base.GetAssociation( associationId ));
        }

        public new Task<Either<Error, IEnumerable<SnAssociation>>> GetAssociations( SnUser forUser ) {
            return Task.Run(() => base.GetAssociations( forUser ));
        }

        public new Task<Either<Error, IEnumerable<SnAssociation>>> GetAssociations( EntityId associationId ) {
            return Task.Run(() => base.GetAssociations( associationId ));
        }
    }
}
