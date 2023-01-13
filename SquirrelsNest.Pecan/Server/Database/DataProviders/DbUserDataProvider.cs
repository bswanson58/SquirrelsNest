using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IUserDataProvider {
        ValueTask<SnUserData>   GetUserData( SnUser forUser, string dataType );
        Task<SnUserData>        UpdateUserData( SnUserData userData );
    }

    public class SnUserDataProvider : ProviderBase<DbUserData>, IUserDataProvider {
        private static SnUserData ConvertTo( DbUserData userData ) => userData.ToEntity();
        private static DbUserData ConvertFrom( SnUserData userData ) => DbUserData.From( userData );

        public SnUserDataProvider( PecanDbContext context )
            : base( context ) { }

        public async ValueTask<SnUserData> GetUserData( SnUser forUser, string dataType ) {
            var dataList = BaseGetAll()
                .Where( d => d.UserId.Equals( forUser.EntityId ) && 
                             d.DataType.Equals( dataType ));

            return ( await dataList
                .Select( d => ConvertTo( d ))
                .ToListAsync())
                .FirstOrDefault( SnUserData.Default );
        }

        public async Task<SnUserData> UpdateUserData( SnUserData userData ) {
            var existingData = await BaseGetAll()
                .Where( d => d.UserId.Equals( userData.UserId ) && 
                             d.DataType.Equals( userData.DataType ))
                .ToListAsync();

            foreach( var data in existingData ) {
                await BaseDelete( data.EntityId );
            }

            return await Create( userData );
        }

        private async Task<SnUserData> Create( SnUserData userData ) =>
            ( await BaseCreate( ConvertFrom( userData ))).ToEntity();
    }
}
