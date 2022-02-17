using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class WorkflowStateProvider : BaseProvider<SnWorkflowState, DbWorkflowState> {
        private static SnWorkflowState ConvertTo( DbWorkflowState state ) => state.ToEntity();
        private static DbWorkflowState ConvertFrom( SnWorkflowState state ) => DbWorkflowState.From( state );

        public WorkflowStateProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.WorkflowStateCollection, ConvertFrom, ConvertTo ) {
        }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbWorkflowState>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnWorkflowState> AddState( SnWorkflowState state ) => Add( state );
        public Either<Error, Unit> UpdateState( SnWorkflowState state ) => Update( state );
        public Either<Error, Unit> DeleteState( SnWorkflowState state ) => Delete( state );
        public Either<Error, SnWorkflowState> GetState( EntityId stateId ) => Get( stateId );
        public Either<Error, IEnumerable<SnWorkflowState>> GetStates() => GetEnumerable();

        public Either<Error, IEnumerable<SnWorkflowState>> GetStates( SnProject forProject ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbWorkflowState.ProjectId ), forProject.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}
