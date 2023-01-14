namespace SquirrelsNest.Pecan.Client.UserData.Actions {
    public class UserDataSetCurrentProjectAction {
        public  string  CurrentProjectId { get; }

        public UserDataSetCurrentProjectAction( string currentProjectId ) {
            CurrentProjectId = currentProjectId;
        }
    }
}
