using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class ExportProjectAction {
        public  SnCompositeProject  Project { get; }

        public ExportProjectAction( SnCompositeProject project ) {
            Project = project;
        }
    }

    public class ExportProjectSubmit {
        public  SnCompositeProject  Project { get; }
        public  bool                IncludeCompletedIssues { get; set; }

        public ExportProjectSubmit( SnCompositeProject project, bool includeCompletedIssues ) {
            IncludeCompletedIssues = includeCompletedIssues;
            Project = project;
        }
    }
}
