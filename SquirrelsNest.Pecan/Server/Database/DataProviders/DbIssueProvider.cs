using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IIssueProvider {
        IQueryable<SnIssue>     GetAll();
        ValueTask<SnIssue ?>    GetById( string id );
        Task<SnIssue>           Create( SnIssue issue );
        ValueTask<SnIssue ?>    Update( SnIssue issue );
        Task                    Delete( SnIssue issue );
    }

    public class SnIssueProvider : ProviderBase<DbIssue>, IIssueProvider {
        private static SnIssue ConvertTo( DbIssue issue ) => issue.ToEntity();
        private static DbIssue ConvertFrom( SnIssue issue ) => DbIssue.From( issue );

        public SnIssueProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnIssue> GetAll() =>
            BaseGetAll().Select( e => ConvertTo( e ));

        public async ValueTask<SnIssue ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnIssue> Create( SnIssue issue ) =>
            ( await BaseCreate( ConvertFrom( issue ))).ToEntity();

        public async ValueTask<SnIssue ?> Update( SnIssue issue ) =>
            ( await BaseUpdate( ConvertFrom( issue )))?.ToEntity();

        public Task Delete( SnIssue issue ) =>
            BaseDelete( issue.EntityId );
    }
}
