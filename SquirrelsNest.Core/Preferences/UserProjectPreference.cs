namespace SquirrelsNest.Core.Preferences {
    public class UserProjectPreference {
        public  string      LastProjectId { get; set; }

        public UserProjectPreference() {
            LastProjectId = String.Empty;
        }

        public UserProjectPreference( string lastProjectId ) {
            LastProjectId = lastProjectId;
        }
    }
}
