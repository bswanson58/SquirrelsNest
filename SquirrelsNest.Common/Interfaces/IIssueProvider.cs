using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IIssueProvider : IDisposable {
        Either<Error, SnIssue>              AddIssue( SnIssue issue );
        Either<Error, Unit>                 UpdateIssue( SnIssue issue );
        Either<Error, Unit>                 DeleteIssue( SnIssue issue );

        Either<Error, SnIssue>              GetIssue( EntityId issueId );
        Either<Error, IEnumerable<SnIssue>> GetIssues();
    }
}
