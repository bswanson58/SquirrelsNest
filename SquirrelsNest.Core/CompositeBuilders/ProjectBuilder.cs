using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;

namespace SquirrelsNest.Core.CompositeBuilders {
    internal class ProjectBuilder : IProjectBuilder {
        private readonly IIssueTypeProvider             mTypeProvider;
        private readonly IComponentProvider             mComponentProvider;
        private readonly IWorkflowStateProvider         mStateProvider;
        private readonly IReleaseProvider               mReleaseProvider;
        private readonly IUserProvider                  mUserProvider;
        private readonly CompositeDisposable            mSubscriptions;
        private readonly Subject<EntitySourceChange>    mChangeSubject;

        public  IObservable<EntitySourceChange>         OnProjectPartsChanged => mChangeSubject.AsObservable();

        public ProjectBuilder( IIssueTypeProvider typeProvider, IWorkflowStateProvider stateProvider, IComponentProvider componentProvider,
                               IReleaseProvider releaseProvider, IUserProvider userProvider ) {
            mTypeProvider = typeProvider;
            mStateProvider = stateProvider;
            mComponentProvider = componentProvider;
            mReleaseProvider = releaseProvider;
            mUserProvider = userProvider;

            mChangeSubject = new Subject<EntitySourceChange>();
            mSubscriptions = new CompositeDisposable();

            mSubscriptions.Add( mTypeProvider.OnEntitySourceChange.Subscribe( OnProviderChange ));
            mSubscriptions.Add( mComponentProvider.OnEntitySourceChange.Subscribe( OnProviderChange ));
            mSubscriptions.Add( mStateProvider.OnEntitySourceChange.Subscribe( OnProviderChange ));
            mSubscriptions.Add( mReleaseProvider.OnEntitySourceChange.Subscribe( OnProviderChange ));
        }

        private void OnProviderChange( EntitySourceChange change ) {
            mChangeSubject.OnNext( change );
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

        public void Dispose() {
            mTypeProvider.Dispose();
            mComponentProvider.Dispose();
            mStateProvider.Dispose();
            mReleaseProvider.Dispose();
            mUserProvider.Dispose();
            mSubscriptions.Dispose();
        }
    }
}
