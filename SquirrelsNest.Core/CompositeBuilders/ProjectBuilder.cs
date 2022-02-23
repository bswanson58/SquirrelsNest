using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class ProjectBuilder : IProjectBuilder {
        private readonly IIssueTypeProvider     mTypeProvider;

        public ProjectBuilder( IIssueTypeProvider typeProvider ) {
            mTypeProvider = typeProvider;
        }

        private IEnumerable<SnIssueType> GetIssueTypes( SnProject forProject ) {
            return mTypeProvider
                .GetIssues( forProject ).Result
                .IfLeft( new List<SnIssueType>());
        }

        public CompositeProject BuildCompositeProject( SnProject forProject ) {
            if( forProject == null ) {
                throw new ArgumentNullException( nameof( forProject ));
            }

            return new CompositeProject( forProject, GetIssueTypes( forProject ));
        }
    }
}
