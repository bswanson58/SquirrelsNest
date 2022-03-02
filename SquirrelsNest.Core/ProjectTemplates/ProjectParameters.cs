namespace SquirrelsNest.Core.ProjectTemplates {
    public class ProjectParameters {
        public  string  ProjectName { get; set; }
        public  string  ProjectDescription { get; set; }
        public  string  ProjectPrefix { get; set; }

        public ProjectParameters() {
            ProjectName = String.Empty;
            ProjectDescription = String.Empty;
            ProjectPrefix = String.Empty;
        }
    }
}
