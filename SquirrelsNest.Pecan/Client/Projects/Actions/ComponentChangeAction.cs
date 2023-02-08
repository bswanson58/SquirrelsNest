using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class ComponentChangeAction {
        public  ComponentChangeInput    Input { get; }

        public ComponentChangeAction( ComponentChangeInput input ) {
            Input = input;
        }
    }

    public class ComponentDeleteAction {
        public  ComponentChangeInput    Input { get; }

        public ComponentDeleteAction( ComponentChangeInput input ) {
            Input = input;
        }
    }

    public class ComponentChangeSubmitAction {
        public  ComponentChangeInput    Input { get; }

        public ComponentChangeSubmitAction( ComponentChangeInput input ) {
            Input = input;
        }
    }

    public class ComponentChangeSuccessAction {
        public  ComponentChangeResponse Response { get; }

        public ComponentChangeSuccessAction( ComponentChangeResponse response ) {
            Response = response;
        }
    }

    public class ComponentChangeFailureAction : FailureAction {
        public ComponentChangeFailureAction( string message ) :
        base( message ) { }
    }
}
