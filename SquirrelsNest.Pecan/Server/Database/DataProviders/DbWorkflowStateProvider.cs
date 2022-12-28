using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IWorkflowStateProvider {
        IQueryable<SnWorkflowState>     GetAll();
        IQueryable<SnWorkflowState>     GetAll( SnProject forProject );
        ValueTask<SnWorkflowState ?>    GetById( string id );
        Task<SnWorkflowState>           Create( SnWorkflowState workflowState );
        ValueTask<SnWorkflowState ?>    Update( SnWorkflowState workflowState );
        Task                            Delete( SnWorkflowState workflowState );
    }

    public class SnWorkflowStateProvider : ProviderBase<DbWorkflowState>, IWorkflowStateProvider {
        private static SnWorkflowState ConvertTo( DbWorkflowState workflowState ) => workflowState.ToEntity();
        private static DbWorkflowState ConvertFrom( SnWorkflowState workflowState ) => DbWorkflowState.From( workflowState );

        public SnWorkflowStateProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnWorkflowState> GetAll() =>
            BaseGetAll().Select( e => ConvertTo( e ));

        public IQueryable<SnWorkflowState> GetAll( SnProject forProject ) =>
            BaseGetAll().Where( e => e.ProjectId.Equals( forProject.EntityId )).Select( e => ConvertTo( e ));

        public async ValueTask<SnWorkflowState ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnWorkflowState> Create( SnWorkflowState workflowState ) =>
            ( await BaseCreate( ConvertFrom( workflowState ))).ToEntity();

        public async ValueTask<SnWorkflowState ?> Update( SnWorkflowState workflowState ) =>
            ( await BaseUpdate( ConvertFrom( workflowState )))?.ToEntity();

        public Task Delete( SnWorkflowState workflowState ) =>
            BaseDelete( workflowState.EntityId );
    }
}
