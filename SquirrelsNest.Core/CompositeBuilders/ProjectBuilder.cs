using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class ProjectBuilder : IProjectBuilder {
        private readonly IIssueTypeProvider     mTypeProvider;
        private readonly IWorkflowStateProvider mStateProvider;

        public ProjectBuilder( IIssueTypeProvider typeProvider, IWorkflowStateProvider stateProvider ) {
            mTypeProvider = typeProvider;
            mStateProvider = stateProvider;
        }

        private IEnumerable<SnIssueType> GetIssueTypes( SnProject forProject ) {
            return mTypeProvider
                .GetIssues( forProject ).Result
                .IfLeft( new List<SnIssueType>());
        }

        private IEnumerable<SnWorkflowState> GetWorkflowStates( SnProject forProject ) {
            return mStateProvider
                .GetStates( forProject ).Result
                .IfLeft( new List<SnWorkflowState>());
        }

        public CompositeProject BuildCompositeProject( SnProject forProject ) {
            if( forProject == null ) {
                throw new ArgumentNullException( nameof( forProject ));
            }

            return new CompositeProject( forProject, GetIssueTypes( forProject ), GetWorkflowStates( forProject ));
        }
    }
}
