using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IIssueTypeProvider {
        IQueryable<SnIssueType>     GetAll();
        IQueryable<SnIssueType>     GetAll( SnProject forProject );
        ValueTask<SnIssueType ?>    GetById( string id );
        Task<SnIssueType>           Create( SnIssueType issueType );
        ValueTask<SnIssueType ?>    Update( SnIssueType issueType );
        Task                        Delete( SnIssueType issueType );
    }

    public class SnIssueTypeProvider : ProviderBase<DbIssueType>, IIssueTypeProvider {
        private static SnIssueType ConvertTo( DbIssueType issueType ) => issueType.ToEntity();
        private static DbIssueType ConvertFrom( SnIssueType issueType ) => DbIssueType.From( issueType );

        public SnIssueTypeProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnIssueType> GetAll() =>
            BaseGetAll().Select( e => ConvertTo( e ));

        public IQueryable<SnIssueType> GetAll( SnProject forProject ) =>
            BaseGetAll().Where( i => i.ProjectId.Equals( forProject.EntityId )).Select( i => ConvertTo( i ));

        public async ValueTask<SnIssueType ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnIssueType> Create( SnIssueType issueType ) =>
            ( await BaseCreate( ConvertFrom( issueType ))).ToEntity();

        public async ValueTask<SnIssueType ?> Update( SnIssueType issueType ) =>
            ( await BaseUpdate( ConvertFrom( issueType )))?.ToEntity();

        public Task Delete( SnIssueType issueType ) =>
            BaseDelete( issueType.EntityId );
    }
}
