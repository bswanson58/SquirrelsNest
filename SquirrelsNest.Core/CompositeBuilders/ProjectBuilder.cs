using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class ProjectBuilder : IProjectBuilder {

        public CompositeProject BuildCompositeProject( SnProject forProject ) {
            if( forProject == null ) {
                throw new ArgumentNullException( nameof( forProject ));
            }

            return new CompositeProject( forProject );
        }
    }
}
