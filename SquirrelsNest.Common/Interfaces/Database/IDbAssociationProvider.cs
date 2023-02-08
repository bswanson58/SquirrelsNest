using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces.Database {
    public interface IDbAssociationProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnAssociation>>              AddAssociation( SnAssociation association );
        Task<Either<Error, Unit>>                       DeleteAssociation( SnAssociation association );

        Task<Either<Error, SnAssociation>>              GetAssociation( EntityId associationId );
        Task<Either<Error, IEnumerable<SnAssociation>>> GetAssociations( SnUser forUser );
        Task<Either<Error, IEnumerable<SnAssociation>>> GetAssociations( EntityId associatedId );
    }
}
