using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Interfaces {
    public interface IProjectBuilder : IDisposable {
        Task<Either<Error, CompositeProject>>   BuildCompositeProject( SnProject forProject );

        IObservable<EntitySourceChange>         OnProjectPartsChanged { get; }
    }
}
