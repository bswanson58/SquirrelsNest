using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory")]
    public class ProjectState : RootState {
        public IReadOnlyList<SnProject> Projects { get; }
        public SnProject ?              CurrentProject { get; }

        public ProjectState( bool callInProgress, string callMessage, 
                             IEnumerable<SnProject> projectList, SnProject ? currentProject ) :
            base( callInProgress, callMessage ) {
            Projects = new List<SnProject>( projectList );
            CurrentProject = currentProject;
        }

        public static ProjectState Factory() => new ( false, string.Empty, Enumerable.Empty<SnProject>(), null );
    }
}
