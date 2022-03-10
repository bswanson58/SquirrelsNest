using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class WorkflowStateProvider : EntityProvider<SnWorkflowState, DbWorkflowState>, IDbWorkflowStateProvider {
        public WorkflowStateProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnWorkflowState ConvertTo( DbWorkflowState state ) => state.ToEntity();
        protected override DbWorkflowState ConvertFrom( SnWorkflowState state ) => DbWorkflowState.From( state );

        public Task<Either<Error, SnWorkflowState>> AddState( SnWorkflowState state ) => AddEntity( state );
        public Task<Either<Error, Unit>> UpdateState( SnWorkflowState state ) => UpdateEntity( state );
        public Task<Either<Error, Unit>> DeleteState( SnWorkflowState state ) => DeleteEntity( state );
        public Task<Either<Error, SnWorkflowState>> GetState( EntityId stateId ) => GetEntity( stateId );
        public Task<Either<Error, IEnumerable<SnWorkflowState>>> GetStates() => GetEntities();
        public Task<Either<Error, IEnumerable<SnWorkflowState>>> GetStates( SnProject forProject ) => 
            GetEntities( c => c.ProjectId.Equals( forProject.EntityId ));
    }
}
