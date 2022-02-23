using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class ProjectBuilder : IProjectBuilder {
        private readonly IIssueTypeProvider     mTypeProvider;
        private readonly IComponentProvider     mComponentProvider;
        private readonly IWorkflowStateProvider mStateProvider;
        private readonly IReleaseProvider       mReleaseProvider;
        private readonly IUserProvider          mUserProvider;

        public ProjectBuilder( IIssueTypeProvider typeProvider, IWorkflowStateProvider stateProvider, IComponentProvider componentProvider,
                               IReleaseProvider releaseProvider, IUserProvider userProvider ) {
            mTypeProvider = typeProvider;
            mStateProvider = stateProvider;
            mComponentProvider = componentProvider;
            mReleaseProvider = releaseProvider;
            mUserProvider = userProvider;
        }

        private IEnumerable<SnIssueType> GetIssueTypes( SnProject forProject ) {
            return mTypeProvider
                .GetIssues( forProject ).Result
                .IfLeft( new List<SnIssueType>());
        }

        private IEnumerable<SnComponent> GetComponents( SnProject forProject ) {
            return mComponentProvider
                .GetComponents( forProject ).Result
                .IfLeft( new List<SnComponent>());
        }

        private IEnumerable<SnWorkflowState> GetWorkflowStates( SnProject forProject ) {
            return mStateProvider
                .GetStates( forProject ).Result
                .IfLeft( new List<SnWorkflowState>());
        }

        private IEnumerable<SnRelease> GetReleases( SnProject forProject ) {
            return mReleaseProvider
                .GetReleases( forProject ).Result
                .IfLeft( new List<SnRelease>());
        }

        private IEnumerable<SnUser> GetUsers() {
            return mUserProvider
                .GetUsers().Result
                .IfLeft( new List<SnUser>());
        }

        public CompositeProject BuildCompositeProject( SnProject forProject ) {
            if( forProject == null ) {
                throw new ArgumentNullException( nameof( forProject ));
            }

            return 
                new CompositeProject( 
                    forProject,
                    GetIssueTypes( forProject ),
                    GetComponents( forProject ),
                    GetWorkflowStates( forProject ),
                    GetReleases( forProject ),
                    GetUsers());
        }
    }
}
