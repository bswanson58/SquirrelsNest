using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IUserProvider {
        IQueryable<SnUser>  GetAll();
        ValueTask<SnUser ?> GetById( string id );
        Task<SnUser>        Create( SnUser user );
        ValueTask<SnUser ?> Update( SnUser user );
        Task                Delete( SnUser user );
    }

    public class SnUserProvider : ProviderBase<DbUser>, IUserProvider {
        private static SnUser ConvertTo( DbUser user ) => user.ToEntity();
        private static DbUser ConvertFrom( SnUser user ) => DbUser.From( user );

        public SnUserProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnUser> GetAll() =>
            BaseGetAll().Select( e => ConvertTo( e ));

        public async ValueTask<SnUser ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnUser> Create( SnUser user ) =>
            ( await BaseCreate( ConvertFrom( user ))).ToEntity();

        public async ValueTask<SnUser ?> Update( SnUser user ) =>
            ( await BaseUpdate( ConvertFrom( user )))?.ToEntity();

        public Task Delete( SnUser user ) =>
            BaseDelete( user.EntityId );
    }
}
