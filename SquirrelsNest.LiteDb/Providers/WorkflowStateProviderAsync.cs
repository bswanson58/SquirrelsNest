using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class WorkflowStateProviderAsync : WorkflowStateProvider, IWorkflowStateProvider {
        public WorkflowStateProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnWorkflowState>> AddState( SnWorkflowState state ) {
            return Task.Run(()=> base.AddState( state ));
        }

        public new Task<Either<Error, Unit>> UpdateState( SnWorkflowState state ) {
            return Task.Run(() => base.UpdateState( state ));
        }

        public new Task<Either<Error, Unit>> DeleteState( SnWorkflowState state ) {
            return Task.Run(() => base.DeleteState( state ));
        }

        public new Task<Either<Error, SnWorkflowState>> GetState( EntityId stateId ) {
            return Task.Run(() => base.GetState( stateId ));
        }

        public new Task<Either<Error, IEnumerable<SnWorkflowState>>> GetStates() {
            return Task.Run(() => base.GetStates());
        }

        public new Task<Either<Error, IEnumerable<SnWorkflowState>>> GetStates( SnProject forProject ) {
            return Task.Run(() => base.GetStates( forProject ));
        }
    }
}
