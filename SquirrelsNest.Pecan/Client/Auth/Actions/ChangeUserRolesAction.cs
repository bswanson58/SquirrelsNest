using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Auth;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Auth.Actions {
    public class ChangeUserRolesAction {
        public  SnUser  User { get; }

        public ChangeUserRolesAction( SnUser user ) {
            User = user;
        }
    }

    public class ChangeUserRolesSubmit {
        public  ChangeUserRolesRequest  Request { get; }

        public ChangeUserRolesSubmit( ChangeUserRolesRequest request ) {
            Request = request;
        }
    }

    public class ChangeUserRolesSuccess {
        public  SnUser  User { get; }

        public ChangeUserRolesSuccess( SnUser user ) {
            User = user;
        }
    }

    public class ChangeUserRolesFailure : FailureAction {
        public ChangeUserRolesFailure( string message )
            : base( message ) { }
    }
}
