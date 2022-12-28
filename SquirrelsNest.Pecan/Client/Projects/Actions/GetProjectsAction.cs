using System.Collections.Generic;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class GetProjectsAction {
    }

    public class GetProjectsSuccessAction {
        public  IEnumerable<SnCompositeProject> Projects { get; }

        public GetProjectsSuccessAction( IEnumerable<SnCompositeProject> projectList ) {
            Projects = projectList;
        }
    }

    public class GetProjectsFailureAction : FailureAction {
        public  GetProjectsFailureAction( string message ) :
            base( message ) {
        }
    }
}
