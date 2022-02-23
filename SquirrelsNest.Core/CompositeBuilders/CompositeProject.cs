using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    public class CompositeProject {
        public  SnProject           Project { get; }

        public CompositeProject( SnProject project ) {
            Project = project;
        }
    }
}
