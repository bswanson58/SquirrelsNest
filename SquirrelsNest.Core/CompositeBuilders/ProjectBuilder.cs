using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt;
using LanguageExt.Common;
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

        public async Task<Either<Error, CompositeProject>> BuildCompositeProject( SnProject forProject ) {
            if( forProject == null ) {
                throw new ArgumentNullException( nameof( forProject ));
            }

            var issueTypes = await mTypeProvider.GetIssues( forProject ).ConfigureAwait( false );
            var components = await mComponentProvider.GetComponents( forProject ).ConfigureAwait( false );
            var states = await mStateProvider.GetStates( forProject ).ConfigureAwait( false );
            var releases = await mReleaseProvider.GetReleases( forProject ).ConfigureAwait( false );
            var users = await mUserProvider.GetUsers().ConfigureAwait( false );
            
            issueTypes = issueTypes.Map( list => list.Append( SnIssueType.Default ));
            components = components.Map( list => list.Append( SnComponent.Default ));
            states = states.Map( list => list.Append( SnWorkflowState.Default ));
            releases = releases.Map( list => list.Append( SnRelease.Default ));
            users = users.Map( list => list.Append( SnUser.Default ));

            return 
                from it in issueTypes
                from c in components
                from s in states
                from r in releases
                from u in users
                select new CompositeProject( forProject, it, c, s, r, u );
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
