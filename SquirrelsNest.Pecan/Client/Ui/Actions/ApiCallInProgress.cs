namespace SquirrelsNest.Pecan.Client.Ui.Actions {
    public class ApiCallStarted {
        public string   AnnouncementMessage { get; }

        public ApiCallStarted( string message ) {
            AnnouncementMessage = message;
        }
    }

    public class ApiCallCompleted { }
}
