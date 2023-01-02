using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Issues;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class AddIssueAction {
        public  SnCompositeProject  Project { get; }

        public AddIssueAction( SnCompositeProject forProject ) {
            Project = forProject;
        }
    }

    public class AddIssueSubmitAction {
        public  CreateIssueRequest  Request {  get; }

        public AddIssueSubmitAction( CreateIssueRequest request ) {
            Request = request;
        }
    }

    public class AddIssueSuccess {
        public  SnCompositeIssue    Issue { get; }

        public AddIssueSuccess( SnCompositeIssue issue ) =>
            Issue = issue;
    }

    public class AddIssueFailure : FailureAction {
        public AddIssueFailure( string message ) :
            base( message ) {
        }
    }
}
