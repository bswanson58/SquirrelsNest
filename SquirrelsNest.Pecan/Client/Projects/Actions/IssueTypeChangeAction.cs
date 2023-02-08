using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class IssueTypeChangeAction {
        public  IssueTypeChangeInput    Input { get; }

        public IssueTypeChangeAction( IssueTypeChangeInput input ) {
            Input = input;
        }
    }

    public class IssueTypeDeleteAction {
        public  IssueTypeChangeInput    Input { get; }

        public IssueTypeDeleteAction( IssueTypeChangeInput input ) {
            Input = input;
        }
    }

    public class IssueTypeChangeSubmitAction {
        public  IssueTypeChangeInput    Input { get; }

        public IssueTypeChangeSubmitAction( IssueTypeChangeInput input ) {
            Input = input;
        }
    }

    public class IssueTypeChangeSuccessAction {
        public  IssueTypeChangeResponse Response { get; }

        public IssueTypeChangeSuccessAction( IssueTypeChangeResponse response ) {
            Response = response;
        }
    }

    public class IssueTypeChangeFailureAction : FailureAction {
        public IssueTypeChangeFailureAction( string message ) :
            base( message ) { }
    }
}
