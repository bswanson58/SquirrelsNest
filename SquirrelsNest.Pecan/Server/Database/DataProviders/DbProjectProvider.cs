using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IProjectProvider {
        IQueryable<SnProject>     GetAll();
        ValueTask<SnProject ?>    GetById( string id );
        Task<SnProject>           Create( SnProject project );
        ValueTask<SnProject ?>    Update( SnProject project );
        Task                    Delete( SnProject project );
    }

    public class SnProjectProvider : ProviderBase<DbProject>, IProjectProvider {
        private static SnProject ConvertTo( DbProject project ) => project.ToEntity();
        private static DbProject ConvertFrom( SnProject project ) => DbProject.From( project );

        public SnProjectProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnProject> GetAll() =>
            BaseGetAll().Select( e => ConvertTo( e ));

        public async ValueTask<SnProject ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnProject> Create( SnProject project ) =>
            ( await BaseCreate( ConvertFrom( project ))).ToEntity();

        public async ValueTask<SnProject ?> Update( SnProject project ) =>
            ( await BaseUpdate( ConvertFrom( project )))?.ToEntity();

        public Task Delete( SnProject project ) =>
            BaseDelete( project.EntityId );
    }
}
