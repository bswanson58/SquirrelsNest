using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class SetCurrentProjectAction {
        public  SnCompositeProject  Project { get; }

        public SetCurrentProjectAction( SnCompositeProject project ) {
            Project = project;
        }
    }
}
