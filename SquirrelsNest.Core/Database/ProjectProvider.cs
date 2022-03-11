using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Database {
    internal class ProjectProvider : BaseDeleteProvider, IProjectProvider {
        private readonly IDbProjectProvider         mProjectProvider;
        private readonly IDbComponentProvider       mComponentProvider;
        private readonly IDbIssueTypeProvider       mIssueTypeProvider;
        private readonly IDbReleaseProvider         mReleaseProvider;
        private readonly IDbWorkflowStateProvider   mStateProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mProjectProvider.OnEntitySourceChange;

        public ProjectProvider( IDbIssueProvider issueProvider, IDbProjectProvider projectProvider,
                                IDbComponentProvider componentProvider, IDbIssueTypeProvider issueTypeProvider,
                                IDbReleaseProvider releaseProvider, IDbWorkflowStateProvider stateProvider ) :
            base( issueProvider ) {
            mProjectProvider = projectProvider;
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mReleaseProvider = releaseProvider;
            mStateProvider = stateProvider;
        }

        public Task<Either<Error, SnProject>> AddProject( SnProject project ) => mProjectProvider.AddProject( project );
        public Task<Either<Error, Unit>> UpdateProject( SnProject project ) => mProjectProvider.UpdateProject( project );
        public Task<Either<Error, SnProject>> GetProject( EntityId projectId ) => mProjectProvider.GetProject( projectId );
        public Task<Either<Error, IEnumerable<SnProject>>> GetProjects() => mProjectProvider.GetProjects();

        public async Task<Either<Error, Unit>> DeleteProject( SnProject project ) {
            if( project == null ) {
                throw new ArgumentNullException( nameof( project ));
            }

            return await CollectProject( project )
                .BindAsync( DeleteProject );
        }

        private async Task<Either<Error, CompositeProject>> CollectProject( SnProject project ) {
            var issueTypes = await mIssueTypeProvider.GetIssues( project ).ConfigureAwait( false );
            var components = await mComponentProvider.GetComponents( project ).ConfigureAwait( false );
            var states = await mStateProvider.GetStates( project ).ConfigureAwait( false );
            var releases = await mReleaseProvider.GetReleases( project ).ConfigureAwait( false );
           
            return 
                from it in issueTypes
                from c in components
                from s in states
                from r in releases
                select new CompositeProject( project, it, c, s, r, Array.Empty<SnUser>() );
        }

        private async Task<Either<Error, Unit>> DeleteProject( CompositeProject project ) {
            var issues = await mIssueProvider.GetIssues( project.Project ).ConfigureAwait( false );

            return await issues.BindAsync( Delete )
                .BindAsync( _ => Delete( project.IssueTypes ))
                .BindAsync( _ => Delete( project.Components ))
                .BindAsync( _ => Delete( project.Releases ))
                .BindAsync( _ => Delete( project.WorkflowStates ))
                .BindAsync( _=> mProjectProvider.DeleteProject( project.Project ));
        }

        private async Task<Either<Error, Unit>> Delete( IEnumerable<SnIssue> issues ) {
            foreach( var issue in issues ) {
                var result = await mIssueProvider.DeleteIssue( issue );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        private async Task<Either<Error, Unit>> Delete( IReadOnlyList<SnIssueType> issueTypes ) {
            foreach( var issueType in issueTypes ) {
                var result = await mIssueTypeProvider.DeleteIssue( issueType );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        private async Task<Either<Error, Unit>> Delete( IReadOnlyList<SnComponent> components ) {
            foreach( var component in components ) {
                var result = await mComponentProvider.DeleteComponent( component );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        private async Task<Either<Error, Unit>> Delete( IReadOnlyList<SnRelease> releases ) {
            foreach( var release in releases ) {
                var result = await mReleaseProvider.DeleteRelease( release );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        private async Task<Either<Error, Unit>> Delete( IReadOnlyList<SnWorkflowState> states ) {
            foreach( var state in states ) {
                var result = await mStateProvider.DeleteState( state );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        public override void Dispose() {
            mComponentProvider.Dispose();
            mIssueTypeProvider.Dispose();
            mReleaseProvider.Dispose();
            mStateProvider.Dispose();

            base.Dispose();
        }
    }
}
