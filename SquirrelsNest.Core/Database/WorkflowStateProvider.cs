using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Core.Database {
    internal class WorkflowStateProvider : BaseDeleteProvider, IWorkflowStateProvider {
        private readonly IDbWorkflowStateProvider   mStateProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mStateProvider.OnEntitySourceChange;

        public WorkflowStateProvider( IDbIssueProvider issueProvider, IDbWorkflowStateProvider stateProvider ) :
            base( issueProvider ) {
            mStateProvider = stateProvider;
        }

        public Task<Either<Error, SnWorkflowState>> AddState( SnWorkflowState state ) => mStateProvider.AddState( state );
        public Task<Either<Error, Unit>> UpdateState( SnWorkflowState state ) => mStateProvider.UpdateState( state );
        public Task<Either<Error, SnWorkflowState>> GetState( EntityId stateId ) => mStateProvider.GetState( stateId );
        public Task<Either<Error, IEnumerable<SnWorkflowState>>> GetStates() => mStateProvider.GetStates();
        public Task<Either<Error, IEnumerable<SnWorkflowState>>> GetStates( SnProject forProject ) => mStateProvider.GetStates( forProject );

        public async Task<Either<Error, Unit>> DeleteState( SnWorkflowState state ) {
            var affected = ( await mIssueProvider.GetIssues().ConfigureAwait( false ))
                .Map( list => from i in list where i.WorkflowStateId.Equals( state.EntityId ) select i )
                .Map( list => from i in list select i.With( SnWorkflowState.Default ));

            return await affected
                .BindAsync( UpdateIssues )
                .BindAsync( _ => mStateProvider.DeleteState( state )).ConfigureAwait( false );
        }

        public override void Dispose() {
            mStateProvider.Dispose();

            base.Dispose();
        }
    }
}
