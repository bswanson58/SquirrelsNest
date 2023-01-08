using System.Collections.Generic;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class LoadIssueListAction {
        public  SnCompositeProject  Project { get; }
        public  PageRequest         PageRequest {  get; }

        public LoadIssueListAction( SnCompositeProject forProject ) {
            Project = forProject;
            PageRequest = new PageRequest( 1, 25 );
        }
    }

    public class LoadIssueListSuccessAction {
        public  PageInformation                 PageInformation { get; }
        public  IEnumerable<SnCompositeIssue>   Issues { get; }

        public LoadIssueListSuccessAction( IEnumerable<SnCompositeIssue> issues, PageInformation pageInformation ) {
            Issues = issues;
            PageInformation = pageInformation;
        }
    }

    public class LoadIssueListFailureAction {
        public  string  Message { get; }

        public LoadIssueListFailureAction( string message ) {
            Message = message;
        }
    }
}
