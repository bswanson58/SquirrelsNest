using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.ProjectTemplates.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory")]
    public class ProjectTemplatesState : RootState {
        public  IReadOnlyList<SnProjectTemplate>    Templates { get; }

        public ProjectTemplatesState( bool callInProgress, string callMessage, IEnumerable<SnProjectTemplate> templates )
            : base( callInProgress, callMessage ) {
            Templates = new List<SnProjectTemplate>( templates );
        }

        public static ProjectTemplatesState Factory() => new ( false, string.Empty, Enumerable.Empty<SnProjectTemplate>());
    }
}
