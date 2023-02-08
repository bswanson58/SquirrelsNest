using System.Collections.Generic;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class GetProjectsAction {
        public  PageRequest PageRequest { get; }

        public GetProjectsAction() {
            PageRequest = new PageRequest( 1, 25 );
        }
    }

    public class GetProjectsSuccessAction {
        public  IEnumerable<SnCompositeProject> Projects { get; }
        public  PageInformation                 PageInformation { get; }

        public GetProjectsSuccessAction( IEnumerable<SnCompositeProject> projectList, PageInformation pageInformation ) {
            Projects = projectList;
            PageInformation = pageInformation;
        }
    }

    public class GetProjectsFailureAction : FailureAction {
        public  GetProjectsFailureAction( string message ) :
            base( message ) {
        }
    }
}
