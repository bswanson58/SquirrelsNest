using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Issues;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class DeleteIssueAction {
        public  SnCompositeIssue    Issue { get; }

        public DeleteIssueAction( SnCompositeIssue issue ) {
            Issue = issue;
        }
    }

    public class DeleteIssueSubmitAction {
        public  DeleteIssueRequest  Request { get; }

        public DeleteIssueSubmitAction( DeleteIssueRequest request ) {
            Request = request;
        }
    }

    public class DeleteIssueSuccess {
        public  SnCompositeIssue    Issue { get; }

        public DeleteIssueSuccess( SnCompositeIssue issue ) {
            Issue = issue;
        }
    }

    public class DeleteIssueFailure : FailureAction {
        public DeleteIssueFailure( string message )
            : base( message ) { }
    }
}
