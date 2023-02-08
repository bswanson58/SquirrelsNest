using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Client.Auth.Actions {
    public class ChangePasswordAction {
        public  ChangePasswordRequest   Request { get; }

        public ChangePasswordAction() {
            Request = new ChangePasswordRequest();
        }
    }

    public class ChangePasswordSubmit {
        public  ChangePasswordRequest   Request { get; }

        public ChangePasswordSubmit( ChangePasswordRequest request ) {
            Request = request;
        }
    }

    public class ChangePasswordSuccess { }

    public class ChangePasswordFailure : FailureAction {
        public ChangePasswordFailure( string message )
            : base( message ) { }
    }
}
