using System.Collections.Generic;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class LoadIssueListAction {
        public  SnCompositeProject  Project { get; }

        public LoadIssueListAction( SnCompositeProject forProject ) {
            Project = forProject;
        }
    }

    public class LoadIssueListSuccessAction {
        public  IEnumerable<SnCompositeIssue>   Issues { get; }

        public LoadIssueListSuccessAction( IEnumerable<SnCompositeIssue> issues ) {
            Issues = issues;
        }
    }

    public class LoadIssueListFailureAction {
        public  string  Message { get; }

        public LoadIssueListFailureAction( string message ) {
            Message = message;
        }
    }
}
