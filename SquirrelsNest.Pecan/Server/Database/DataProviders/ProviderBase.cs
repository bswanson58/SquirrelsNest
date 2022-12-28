using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SquirrelsNest.Pecan.Server.Database.Entities;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public class ProviderBase<TEntity> where TEntity : DbEntityBase<TEntity> {
        private readonly PecanDbContext mDbContext;

        public ProviderBase( PecanDbContext context ) {
            mDbContext = context;
        }

        protected IQueryable<TEntity> BaseGetAll() => mDbContext.Set<TEntity>().AsNoTracking();

        protected ValueTask<TEntity ?> BaseGetById( string id ) => mDbContext.FindAsync<TEntity>( id );

        protected async Task<TEntity> BaseCreate( TEntity entity ) {
            await mDbContext.Set<TEntity>().AddAsync( entity );
            await mDbContext.SaveChangesAsync();

            return entity;
        }

        protected async ValueTask<TEntity ?> BaseUpdate( TEntity entity ) {
            var current = await BaseGetById( entity.EntityId );

            if( current != null ) {
                current.Update( entity );
                mDbContext.Set<TEntity>().Update( entity );

                await mDbContext.SaveChangesAsync();
            }

            return current;
        }

        protected async Task BaseDelete( string id ) {
            var entity = await BaseGetById( id );

            if( entity != null ) {
                mDbContext.Set<TEntity>().Remove( entity );
                await mDbContext.SaveChangesAsync();
            }
        }
    }
}
