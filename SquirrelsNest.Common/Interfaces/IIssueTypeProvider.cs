using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IIssueTypeProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnIssueType>>                AddIssue( SnIssueType issue );
        Task<Either<Error, Unit>>                       UpdateIssue( SnIssueType issue );
        Task<Either<Error, Unit>>                       DeleteIssue( SnIssueType issue );

        Task<Either<Error, SnIssueType>>                GetIssue( EntityId issueTypeId );
        Task<Either<Error, IEnumerable<SnIssueType>>>   GetIssues();
        Task<Either<Error, IEnumerable<SnIssueType>>>   GetIssues( SnProject forProject );
    }
}