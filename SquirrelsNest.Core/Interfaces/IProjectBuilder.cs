using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Interfaces {
    public interface IProjectBuilder : IDisposable {
        CompositeProject                    BuildCompositeProject( SnProject forProject );

        IObservable<EntitySourceChange>     OnProjectPartsChanged { get; }
    }
}
