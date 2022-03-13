using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Database {
    internal class ProjectProvider : BaseDeleteProvider, IProjectProvider {
        private readonly IDbAssociationProvider     mAssociationProvider;
        private readonly IDbProjectProvider         mProjectProvider;
        private readonly IDbComponentProvider       mComponentProvider;
        private readonly IDbIssueTypeProvider       mIssueTypeProvider;
        private readonly IDbReleaseProvider         mReleaseProvider;
        private readonly IDbWorkflowStateProvider   mStateProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mProjectProvider.OnEntitySourceChange;

        public ProjectProvider( IDbAssociationProvider associationProvider, IDbIssueProvider issueProvider, IDbProjectProvider projectProvider,
                                IDbComponentProvider componentProvider, IDbIssueTypeProvider issueTypeProvider,
                                IDbReleaseProvider releaseProvider, IDbWorkflowStateProvider stateProvider ) :
            base( issueProvider ) {
            mAssociationProvider = associationProvider;
            mProjectProvider = projectProvider;
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mReleaseProvider = releaseProvider;
            mStateProvider = stateProvider;
        }

        public async Task<Either<Error, SnProject>> AddProject( SnProject project, SnUser forUser ) {
            var projectResult = await mProjectProvider.AddProject( project ).ConfigureAwait( false );

            return await projectResult.BindAsync( async p => {
                var association = new SnAssociation( forUser.EntityId, p.EntityId );

                return ( await mAssociationProvider.AddAssociation( association ).ConfigureAwait( false ))
                    .Bind( _ => projectResult.Map( pr => pr ));
            }); 
        }

        public Task<Either<Error, Unit>> UpdateProject( SnProject project ) => mProjectProvider.UpdateProject( project );
        public Task<Either<Error, SnProject>> GetProject( EntityId projectId ) => mProjectProvider.GetProject( projectId );

        public async Task<Either<Error, IEnumerable<SnProject>>> GetProjects( SnUser forUser ) {
            var associatedProjects = new List<EntityId>();
            var projects = await mProjectProvider.GetProjects().ConfigureAwait( false );

            ( await mAssociationProvider.GetAssociations( forUser ).ConfigureAwait( false ))
                .Do( list => associatedProjects.AddRange( from a in list select a.AssociationId ));

            return projects.Map( list => from project in list where associatedProjects.Contains( project.EntityId ) select project );
        }

        private async Task<Either<Error, Unit>> DeleteProject( SnProject project ) {
            if( project == null ) {
                throw new ArgumentNullException( nameof( project ));
            }

            return await CollectProject( project )
                .BindAsync( DeleteProject );
        }

        private async Task<Either<Error, Unit>> DeleteAssociations( IEnumerable<SnAssociation> associations ) {
            foreach( var association in associations ) {
                var result = await mAssociationProvider.DeleteAssociation( association );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        public async Task<Either<Error, Unit>> DeleteProject( SnProject project, SnUser user ) {
            var associations = ( await mAssociationProvider.GetAssociations( user ))
                .Map( list => from a in list where a.AssociationId.Equals( project.EntityId ) select a );

            var projectDelete = await DeleteProject( project );

            if( projectDelete.IsRight ) {
                return await associations.BindAsync( DeleteAssociations );
            }

            return projectDelete;
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
