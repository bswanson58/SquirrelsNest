using Microsoft.AspNetCore.Components.Forms;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class ImportProjectAction {
    }

    public class ImportProjectSubmit {
        public  ImportProjectRequest    Request { get; }
        public  IBrowserFile ?          File { get; set; }

        public ImportProjectSubmit( ImportProjectRequest request ) {
            Request = request;
            File = null;
        }
    }

    public class ImportProjectSuccess {
        public  SnCompositeProject      Project { get; }

        public ImportProjectSuccess( SnCompositeProject project ) {
            Project = project;
        }
    }

    public class ImportProjectFailure : FailureAction {
        public ImportProjectFailure( string message )
            : base( message ) { }
    }
}
