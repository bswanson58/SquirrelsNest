using System.Diagnostics;
using SquirrelsNest.Common.Entities;
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SquirrelsNest.Core.ProjectTemplates {
    public class EntityDescription {
        public  string  Name { get; set; }
        public  string  Description { get; set; }

        protected EntityDescription() {
            Name = String.Empty;
            Description = String.Empty;
        }
    }

    [DebuggerDisplay("Component: {" + nameof( Name ) + "}")]
    public class ComponentDescription : EntityDescription {
        internal static ComponentDescription From( SnComponent component ) =>
            new () { Name = component.Name, Description = component.Description };
    }

    [DebuggerDisplay("IssueType: {" + nameof( Name ) + "}")]
    public class IssueTypeDescription : EntityDescription {
        internal static IssueTypeDescription From( SnIssueType issueType ) => 
            new () { Name = issueType.Name, Description = issueType.Description };
    }

    [DebuggerDisplay("Workflow: {" + nameof( Name ) + "}")]
    public class WorkflowStepDescription : EntityDescription {
        public  StateCategory   Category { get; set; }

        internal static WorkflowStepDescription From( SnWorkflowState state ) =>
            new () {
                Name = state.Name,
                Description = state.Description,
                Category = state.Category
            };
    }

    [DebuggerDisplay("Template: {" + nameof( TemplateName ) + "}")]
    public class ProjectTemplate {
        public  string                          TemplateName { get; set; }
        public  string                          TemplateDescription { get; set; }

        public  List<IssueTypeDescription>      IssueTypes { get; set; }
        public  List<ComponentDescription>      Components { get; set; }
        public  List<WorkflowStepDescription>   WorkflowSteps { get; set; }

        public ProjectTemplate() {
            TemplateName = String.Empty;
            TemplateDescription = String.Empty;
            IssueTypes = new List<IssueTypeDescription>();
            Components = new List<ComponentDescription>();
            WorkflowSteps = new List<WorkflowStepDescription>();
        }
    }
}
