using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client.UserData.Actions {
    public class IssueDisplayCompleted {
        public bool DisplayCompleted { get; }

        public IssueDisplayCompleted( bool displayCompleted ) {
            DisplayCompleted = displayCompleted;
        }
    }

    public class IssueDisplayCompletedLast {
        public bool DisplayCompletedLast { get; }

        public IssueDisplayCompletedLast( bool displayCompletedLast ) {
            DisplayCompletedLast = displayCompletedLast;
        }
    }

    public class IssueDisplayMyAssigned {
        public bool DisplayMyAssigned { get; }

        public IssueDisplayMyAssigned( bool displayMyAssigned ) {
            DisplayMyAssigned = displayMyAssigned;
        }
    }

    public class IssueListDisplayStyleAction {
        public  string  IssueListDisplayStyle { get; }

        public IssueListDisplayStyleAction( string style ) {
            IssueListDisplayStyle = IssueListStyle.FullDetail;

            if(( style.Equals( IssueListStyle.TitleOnly )) ||
               ( style.Equals( IssueListStyle.TitleDescription )) ||
               ( style.Equals( IssueListStyle.FullDetail ))) {
                IssueListDisplayStyle = style;
            }
        }
    }
}
