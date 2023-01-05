using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Issues;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class EditIssueAction {
        public  SnCompositeProject  Project { get; }
        public  SnCompositeIssue    Issue { get; }

        public EditIssueAction( SnCompositeProject project, SnCompositeIssue issue ) {
            Project = project;
            Issue = issue;
        }
    }

    public class UpdateIssueSubmit {
        public UpdateIssueRequest   Request { get; }

        public UpdateIssueSubmit( UpdateIssueRequest request ) {
            Request = request;
        }
    }

    public class UpdateIssueSuccess {
        public  SnCompositeIssue    Issue { get; }

        public UpdateIssueSuccess( SnCompositeIssue issue ) {
            Issue = issue;
        }
    }

    public class UpdateIssueFailure : FailureAction {
        public UpdateIssueFailure( string message ) :
            base( message ) { }
    }
}
