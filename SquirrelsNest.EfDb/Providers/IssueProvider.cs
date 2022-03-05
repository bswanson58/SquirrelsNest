using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class IssueProvider : EntityProvider<SnIssue, DbIssue>, IIssueProvider {
        public IssueProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnIssue ConvertTo( DbIssue issue ) => issue.ToEntity();
        protected override DbIssue ConvertFrom( SnIssue issue ) => DbIssue.From( issue );

        public Task<Either<Error, SnIssue>> AddIssue( SnIssue issue ) => AddEntity( issue );
        public Task<Either<Error, Unit>> UpdateIssue( SnIssue issue ) => UpdateEntity( issue );
        public Task<Either<Error, Unit>> DeleteIssue( SnIssue issue ) => DeleteEntity( issue );
        public Task<Either<Error, SnIssue>> GetIssue( EntityId issueId ) => GetEntity( issueId );
        public Task<Either<Error, IEnumerable<SnIssue>>> GetIssues() => GetEntities();
        public Task<Either<Error, IEnumerable<SnIssue>>> GetIssues( SnProject forProject ) => 
            GetEntities( c => c.ProjectId.Equals( forProject.EntityId ));
    }
}
