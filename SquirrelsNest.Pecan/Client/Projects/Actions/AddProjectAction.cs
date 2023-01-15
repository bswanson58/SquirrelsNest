using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class AddProjectAction {
    }

    public class AddProjectSubmitAction {
        public  CreateProjectRequest  ProjectRequest {  get; }

        public AddProjectSubmitAction( CreateProjectRequest projectRequest ) {
            ProjectRequest = projectRequest;
        }
    }

    public class AddProjectFromTemplateSubmitAction {
        public  CreateProjectRequest  ProjectRequest {  get; }

        public AddProjectFromTemplateSubmitAction( CreateProjectRequest projectRequest ) {
            ProjectRequest = projectRequest;
        }
    }

    public class AddProjectSuccess {
        public  SnCompositeProject  Project { get; }

        public AddProjectSuccess( SnCompositeProject project ) => Project = project;
    }

    public class AddProjectFailure : FailureAction {
        public AddProjectFailure( string message ) :
            base( message ) { }
    }
}
