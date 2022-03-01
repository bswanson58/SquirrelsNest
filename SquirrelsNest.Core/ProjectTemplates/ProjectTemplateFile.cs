namespace SquirrelsNest.Core.ProjectTemplates {
// ReSharper disable UnusedAutoPropertyAccessor.Global

    internal class EntityDescription {
        public  string  Name { get; set; }
        public  string  Description { get; set; }

        public EntityDescription() {
            Name = String.Empty;
            Description = String.Empty;
        }
    }

    public interface IProjectTemplate {
        string  Name { get; }
        string  Description { get; }
    }

    internal class ProjectTemplateFile : IProjectTemplate {
        public  string                      Name { get; set; }
        public  string                      Description { get; set; }

        public  List<EntityDescription>     IssueTypes { get; set; }
        public  List<EntityDescription>     Components { get; set; }
        public  List<EntityDescription>     WorkflowSteps { get; set; }

        public ProjectTemplateFile() {
            Name = String.Empty;
            Description = String.Empty;
            IssueTypes = new List<EntityDescription>();
            Components = new List<EntityDescription>();
            WorkflowSteps = new List<EntityDescription>();
        }
    }
}
