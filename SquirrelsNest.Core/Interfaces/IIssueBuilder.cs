using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Interfaces {
    public interface IIssueBuilder {
        Task<Either<Error, CompositeIssue>> BuildCompositeIssue( SnIssue forIssue );
    }
}
