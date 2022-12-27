using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Actions {
    public class SetCurrentProjectAction {
        public  SnProject   Project { get; }

        public SetCurrentProjectAction( SnProject project ) {
            Project = project;
        }
    }
}
