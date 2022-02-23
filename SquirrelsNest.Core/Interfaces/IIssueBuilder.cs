using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Interfaces {
    public interface IIssueBuilder {
        CompositeIssue  BuildCompositeIssue( SnIssue forIssue );
    }
}
