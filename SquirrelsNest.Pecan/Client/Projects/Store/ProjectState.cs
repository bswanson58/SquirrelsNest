using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory")]
    public class ProjectState : RootState {
        public IReadOnlyList<SnCompositeProject>    Projects { get; }
        public SnCompositeProject ?                 CurrentProject { get; }

        public ProjectState( bool callInProgress, string callMessage, 
                             IEnumerable<SnCompositeProject> projectList, SnCompositeProject ? currentProject ) :
            base( callInProgress, callMessage ) {
            Projects = new List<SnCompositeProject>( projectList );
            CurrentProject = currentProject;
        }

        public static ProjectState Factory() => new ( false, string.Empty, Enumerable.Empty<SnCompositeProject>(), null );
    }
}
