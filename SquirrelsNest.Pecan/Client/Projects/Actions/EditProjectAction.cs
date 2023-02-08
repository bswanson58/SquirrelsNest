using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class EditProjectAction {
        public  SnCompositeProject  Project { get; }

        public EditProjectAction( SnCompositeProject project ) {
            Project = project;
        }
    }

    public class EditProjectSubmit {
        public  EditProjectRequest  Request { get; }

        public EditProjectSubmit( EditProjectRequest request ) {
            Request = request;
        }
    }

    public class EditProjectSuccess {
        public  SnCompositeProject  Project { get; }

        public EditProjectSuccess( SnCompositeProject project ) {
            Project = project;
        }
    }

    public class EditProjectFailure : FailureAction {
        public EditProjectFailure( string message )
            : base( message ) { }
    }
}
