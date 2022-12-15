using System.Linq;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IComponentProvider {
        IQueryable<SnComponent>     GetAll();
        ValueTask<SnComponent ?>    GetById( string id );
        Task<SnComponent>           Create( SnComponent component );
        ValueTask<SnComponent ?>    Update( SnComponent component );
        Task                        Delete( SnComponent component );
    }

    public class SnComponentProvider : ProviderBase<DbComponent>, IComponentProvider {
        private static SnComponent ConvertTo( DbComponent component ) => component.ToEntity();
        private static DbComponent ConvertFrom( SnComponent component ) => DbComponent.From( component );

        public SnComponentProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnComponent> GetAll() =>
            BaseGetAll().Select( e => ConvertTo( e ));

        public async ValueTask<SnComponent ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnComponent> Create( SnComponent component ) =>
            ( await BaseCreate( ConvertFrom( component ))).ToEntity();

        public async ValueTask<SnComponent ?> Update( SnComponent component ) =>
            ( await BaseUpdate( ConvertFrom( component )))?.ToEntity();

        public Task Delete( SnComponent component ) =>
            BaseDelete( component.EntityId );
    }
}
