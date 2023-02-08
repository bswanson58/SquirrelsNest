using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class CreateProjectTemplateAction {
        public  SnCompositeProject  Project { get; }

        public CreateProjectTemplateAction( SnCompositeProject  project ) {
            Project = project;
        }
    }

    public class CreateProjectTemplateSubmit {
        public  CreateProjectTemplateRequest    Request { get; }

        public CreateProjectTemplateSubmit( CreateProjectTemplateRequest request ) {
            Request = request;
        }
    }

    public class CreateProjectTemplateSuccess {
        public  SnProjectTemplate   Template { get; }

        public CreateProjectTemplateSuccess( SnProjectTemplate template ) {
            Template = template;
        }
    }

    public class CreateProjectTemplateFailure : FailureAction {
        public CreateProjectTemplateFailure( string message )
            : base( message ) { }
    }
}
