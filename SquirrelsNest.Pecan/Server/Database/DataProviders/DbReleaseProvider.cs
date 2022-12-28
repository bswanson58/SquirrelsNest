using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IReleaseProvider {
        IQueryable<SnRelease>   GetAll();
        IQueryable<SnRelease>   GetAll( SnProject forProject );
        ValueTask<SnRelease ?>  GetById( string id );
        Task<SnRelease>         Create( SnRelease release );
        ValueTask<SnRelease ?>  Update( SnRelease release );
        Task                    Delete( SnRelease release );
    }

    public class SnReleaseProvider : ProviderBase<DbRelease>, IReleaseProvider {
        private static SnRelease ConvertTo( DbRelease release ) => release.ToEntity();
        private static DbRelease ConvertFrom( SnRelease release ) => DbRelease.From( release );

        public SnReleaseProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnRelease> GetAll() =>
            BaseGetAll().Select( e => ConvertTo( e ));

        public IQueryable<SnRelease> GetAll( SnProject forProject ) =>
            BaseGetAll().Where( e => e.ProjectId.Equals( forProject.EntityId )).Select( e => ConvertTo( e ));

        public async ValueTask<SnRelease ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnRelease> Create( SnRelease release ) =>
            ( await BaseCreate( ConvertFrom( release ))).ToEntity();

        public async ValueTask<SnRelease ?> Update( SnRelease release ) =>
            ( await BaseUpdate( ConvertFrom( release )))?.ToEntity();

        public Task Delete( SnRelease release ) =>
            BaseDelete( release.EntityId );
    }
}
