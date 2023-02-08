using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class WorkflowStateChangeAction {
        public  WorkflowStateChangeInput    Input { get; }

        public WorkflowStateChangeAction( WorkflowStateChangeInput input ) {
            Input = input;
        }
    }

    public class WorkflowStateDeleteAction {
        public  WorkflowStateChangeInput    Input { get; }

        public WorkflowStateDeleteAction( WorkflowStateChangeInput input ) {
            Input = input;
        }
    }

    public class WorkflowStateChangeSubmitAction {
        public  WorkflowStateChangeInput    Input { get; }

        public WorkflowStateChangeSubmitAction( WorkflowStateChangeInput input ) {
            Input = input;
        }
    }

    public class WorkflowStateChangeSuccessAction {
        public  WorkflowStateChangeResponse Response { get; }

        public WorkflowStateChangeSuccessAction( WorkflowStateChangeResponse response ) {
            Response = response;
        }
    }

    public class WorkflowStateChangeFailureAction : FailureAction {
        public WorkflowStateChangeFailureAction( string message ) :
            base( message ) { }
    }
}
