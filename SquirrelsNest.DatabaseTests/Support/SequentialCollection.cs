using Xunit;

namespace SquirrelsNest.DatabaseTests.Support {
    [CollectionDefinition(nameof(SequentialCollection), DisableParallelization = true)]
    public class SequentialCollection { }
}
