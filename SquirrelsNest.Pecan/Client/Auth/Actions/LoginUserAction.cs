using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Client.Auth.Actions {
    public class LoginUserAction {
    }

    public class LoginUserSubmitAction {
        public  LoginUserInput     UserInput {  get; }

        public LoginUserSubmitAction( LoginUserInput userInput ) {
            UserInput = userInput;
        }
    }

    public class LoginUserSuccessAction {
        public LoginUserResponse    UserResponse { get; }

        public LoginUserSuccessAction( LoginUserResponse response ) {
            UserResponse = response;
        }
    }

    public class LoginUserFailureAction : FailureAction {
        public LoginUserFailureAction( string message ) :
            base( message ) { }
    }
}
