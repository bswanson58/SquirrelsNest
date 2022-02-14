using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IIssueProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnIssue>>                AddIssue( SnIssue issue );
        Task<Either<Error, Unit>>                   UpdateIssue( SnIssue issue );
        Task<Either<Error, Unit>>                   DeleteIssue( SnIssue issue );

        Task<Either<Error, SnIssue>>                GetIssue( EntityId issueId );
        Task<Either<Error, IEnumerable<SnIssue>>>   GetIssues();
        Task<Either<Error, IEnumerable<SnIssue>>>   GetIssues( SnProject forProject );
    }
}
