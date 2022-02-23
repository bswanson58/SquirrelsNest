using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class IssueBuilder : IIssueBuilder {
        private readonly IIssueTypeProvider     mIssueTypeProvider;

        public IssueBuilder( IIssueTypeProvider issueTypeProvider ) {
            mIssueTypeProvider = issueTypeProvider;
        }

        private SnIssueType GetIssueType( SnIssue issue ) {
            return mIssueTypeProvider
                .GetIssue( issue.IssueTypeId ).Result
                .IfLeft( SnIssueType.Default );
        }

        public CompositeIssue BuildCompositeIssue( SnIssue forIssue ) {
            return new CompositeIssue( forIssue, GetIssueType( forIssue ));
        }
    }
}
