using SquirrelsNest.Pecan.Server.Database.Entities;
using System.Linq;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IAssociationProvider {
        IQueryable<SnAssociation>   GetAll();
        ValueTask<SnAssociation ?>  GetById( string id );
        Task<SnAssociation>         Create( SnAssociation association );
        ValueTask<SnAssociation ?>  Update( SnAssociation association );
        Task                        Delete( SnAssociation association );
    }

    public class DbAssociationProvider : ProviderBase<DbAssociation>, IAssociationProvider {
        private static SnAssociation ConvertTo( DbAssociation association ) => association.ToEntity();
        private static DbAssociation ConvertFrom( SnAssociation association ) => DbAssociation.From( association );

        public DbAssociationProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnAssociation> GetAll() => 
            BaseGetAll().Select( e => ConvertTo( e ));

        public async ValueTask<SnAssociation ?> GetById( string id ) => 
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnAssociation> Create( SnAssociation association ) => 
            ( await BaseCreate( ConvertFrom( association ))).ToEntity();

        public async ValueTask<SnAssociation ?> Update( SnAssociation association ) => 
            ( await BaseUpdate( ConvertFrom( association )))?.ToEntity();

        public Task Delete( SnAssociation association ) => 
            BaseDelete( association.EntityId );
    }
}
