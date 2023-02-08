namespace SquirrelsNest.Pecan.Client.UserData.Actions {
    public class RequestUserDataAction {
    }

    public class RequestUserDataSuccess {
        public  PecanUserData   UserData { get; }

        public RequestUserDataSuccess( PecanUserData userData ) {
            UserData = userData;
        }
    }
}
