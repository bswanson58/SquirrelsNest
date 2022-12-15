using System.Linq;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IUserDataProvider {
        IQueryable<SnUserData>  GetAll();
        ValueTask<SnUserData ?> GetById( string id );
        Task<SnUserData>        Create( SnUserData userData );
        ValueTask<SnUserData ?> Update( SnUserData userData );
        Task                    Delete( SnUserData userData );
    }

    public class SnUserDataProvider : ProviderBase<DbUserData>, IUserDataProvider {
        private static SnUserData ConvertTo( DbUserData userData ) => userData.ToEntity();
        private static DbUserData ConvertFrom( SnUserData userData ) => DbUserData.From( userData );

        public SnUserDataProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnUserData> GetAll() =>
            BaseGetAll().Select( e => ConvertTo( e ));

        public async ValueTask<SnUserData ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnUserData> Create( SnUserData userData ) =>
            ( await BaseCreate( ConvertFrom( userData ))).ToEntity();

        public async ValueTask<SnUserData ?> Update( SnUserData userData ) =>
            ( await BaseUpdate( ConvertFrom( userData )))?.ToEntity();

        public Task Delete( SnUserData userData ) =>
            BaseDelete( userData.EntityId );
    }
}
