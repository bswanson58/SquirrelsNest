using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Common.Interfaces {
    public interface IEntityChangeNotifier {
        IObservable<EntitySourceChange>     OnEntitySourceChange { get; }
    }
}
