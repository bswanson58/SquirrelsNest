using SquirrelsNest.Pecan.Server.Database.Entities;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Database.DataProviders {
    public interface IIssueProvider {
        IQueryable<SnIssue>     GetAll( SnProject forProject );
        ValueTask<SnIssue ?>    GetById( string id );
        Task<SnIssue>           Create( SnIssue issue );
        ValueTask<SnIssue ?>    Update( SnIssue issue );
        Task                    Delete( SnIssue issue );
        Task                    Delete( string issueId );
    }

    public class SnIssueProvider : ProviderBase<DbIssue>, IIssueProvider {
        private static SnIssue ConvertTo( DbIssue issue ) => issue.ToEntity();
        private static DbIssue ConvertFrom( SnIssue issue ) => DbIssue.From( issue );

        public SnIssueProvider( PecanDbContext context )
            : base( context ) { }

        public IQueryable<SnIssue> GetAll( SnProject forProject ) =>
            BaseGetAll().Where( p => p.ProjectId.Equals( forProject.EntityId )).Select( e => ConvertTo( e ));

        public async ValueTask<SnIssue ?> GetById( string id ) =>
            ( await BaseGetById( id ))?.ToEntity();

        public async Task<SnIssue> Create( SnIssue issue ) =>
            ( await BaseCreate( ConvertFrom( issue ))).ToEntity();

        public async ValueTask<SnIssue ?> Update( SnIssue issue ) =>
            ( await BaseUpdate( ConvertFrom( issue )))?.ToEntity();

        public Task Delete( SnIssue issue ) =>
            BaseDelete( issue.EntityId );

        public Task Delete( string issueId ) =>
            BaseDelete( issueId );
    }
}
