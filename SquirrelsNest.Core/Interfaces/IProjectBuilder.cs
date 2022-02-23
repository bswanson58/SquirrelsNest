using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Interfaces {
    public interface IProjectBuilder {
        CompositeProject    BuildCompositeProject( SnProject forProject );
    }
}
