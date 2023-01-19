using System.Collections.Generic;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class PrepareForNewProjectAction {
        public  string                  ProjectId { get; }
        public  List<SnCompositeIssue>  Issues { get; }
        public  PageInformation         PageInformation { get; }
        public  uint                    CurrentDisplayPage { get; }

        public PrepareForNewProjectAction( string projectId, PageInformation pageInformation, 
                                           uint currentDisplayPage, IEnumerable<SnCompositeIssue> issues ) {
            ProjectId = projectId;
            PageInformation = pageInformation;
            CurrentDisplayPage = currentDisplayPage;
            Issues = new List<SnCompositeIssue>( issues );
        }
    }
}
