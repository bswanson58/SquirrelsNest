using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IComponentProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnComponent>>                AddComponent( SnComponent component );
        Task<Either<Error, Unit>>                       UpdateComponent( SnComponent component );
        Task<Either<Error, Unit>>                       DeleteComponent( SnComponent component );

        Task<Either<Error, SnComponent>>                GetComponent( EntityId componentId );
        Task<Either<Error, IEnumerable<SnComponent>>>   GetComponents();
        Task<Either<Error, IEnumerable<SnComponent>>>   GetComponents( SnProject forProject );
    }
}
