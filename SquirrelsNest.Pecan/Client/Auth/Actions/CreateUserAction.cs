using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto;

namespace SquirrelsNest.Pecan.Client.Auth.Actions {
    public class CreateUserAction {
    }

    public class CreateUserSubmitAction {
        public  CreateUserInput     UserInput {  get; }

        public CreateUserSubmitAction( CreateUserInput userInput ) {
            UserInput = userInput;
        }
    }

    public class CreateUserSuccessAction {
    }

    public class CreateUserFailureAction : FailureAction {
        public CreateUserFailureAction( string message ) :
            base( message ) { }
    }
}
