using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects {
    [FeatureState( CreateInitialStateMethodName = "Factory")]
    public class ProjectState : RootState {
        public  IReadOnlyList<SnProject>    Projects { get; }

        public ProjectState( bool callInProgress, string callMessage, IEnumerable<SnProject> projectList ) :
            base( callInProgress, callMessage ) {
            Projects = new List<SnProject>( projectList );
        }

        public static ProjectState Factory() => new ProjectState( false, String.Empty, Enumerable.Empty<SnProject>());
    }
}
