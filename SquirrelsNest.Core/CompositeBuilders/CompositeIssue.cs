using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    public class CompositeIssue {
        public  SnIssue         Issue { get; }
        public  SnIssueType     IssueType { get; }

        public CompositeIssue( SnIssue issue, SnIssueType issueType ) {
            Issue = issue;
            IssueType = issueType;
        }
    }
}
