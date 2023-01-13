namespace SquirrelsNest.Pecan.Client.Ui.Actions {
    public class IssueDisplayCompleted {
        public  bool    DisplayCompleted { get; }

        public IssueDisplayCompleted( bool displayCompleted ) {
            DisplayCompleted = displayCompleted;
        }
    }

    public class IssueDisplayCompletedLast {
        public  bool    DisplayCompletedLast { get; }

        public IssueDisplayCompletedLast( bool displayCompletedLast ) {
            DisplayCompletedLast = displayCompletedLast;
        }
    }

    public class IssueDisplayMyAssigned {
        public  bool    DisplayMyAssigned {  get; }

        public IssueDisplayMyAssigned( bool displayMyAssigned ) {
            DisplayMyAssigned = displayMyAssigned;
        }
    }
}
