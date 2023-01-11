using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class DeleteProjectAction {
        public  SnCompositeProject      Project { get; }

        public DeleteProjectAction( SnCompositeProject project ) {
            Project = project;
        }
    }

    public class DeleteProjectSubmit {
        public  DeleteProjectRequest    Request { get; }

        public DeleteProjectSubmit( DeleteProjectRequest request ) {
            Request = request;
        }
    }

    public class DeleteProjectSuccess {
        public  SnCompositeProject      Project { get; }

        public DeleteProjectSuccess( SnCompositeProject project ) {
            Project = project;
        }
    }

    public class DeleteProjectFailure : FailureAction {
        public DeleteProjectFailure( string message )
            : base( message ) { }
    }
}
