using System.Collections.Generic;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class GetProjectsAction {
    }

    public class GetProjectsSuccessAction {
        public  IEnumerable<SnProject>  Projects { get; }

        public GetProjectsSuccessAction( IEnumerable<SnProject> projectList ) {
            Projects = projectList;
        }
    }

    public class GetProjectsFailureAction : FailureAction {
        public  GetProjectsFailureAction( string message ) :
            base( message ) {
        }
    }
}
