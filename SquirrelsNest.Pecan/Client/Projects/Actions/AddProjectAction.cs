using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class AddProjectAction {
    }

    public class AddProjectSubmitAction {
        public  CreateProjectInput  ProjectInput {  get; }

        public AddProjectSubmitAction( CreateProjectInput projectInput ) {
            ProjectInput = projectInput;
        }
    }

    public class AddProjectSuccess {
        public  SnProject   Project { get; }

        public AddProjectSuccess( SnProject project ) => Project = project;
    }

    public class AddProjectFailure : FailureAction {
        public AddProjectFailure( string message ) :
            base( message ) { }
    }
}
