using System;

namespace SquirrelsNest.Pecan.Server.Features.ProjectTemplates {
    public class ProjectParameters {
        public  string  ProjectName { get; set; }
        public  string  ProjectDescription { get; set; }
        public  string  IssuePrefix { get; set; }

        public ProjectParameters() {
            ProjectName = String.Empty;
            ProjectDescription = String.Empty;
            IssuePrefix = String.Empty;
        }
    }
}
