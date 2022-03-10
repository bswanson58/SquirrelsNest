using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class IssueTypeProvider : EntityProvider<SnIssueType, DbIssueType>, IDbIssueTypeProvider {
        public IssueTypeProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnIssueType ConvertTo( DbIssueType issueType ) => issueType.ToEntity();
        protected override DbIssueType ConvertFrom( SnIssueType issueType ) => DbIssueType.From( issueType );

        public Task<Either<Error, SnIssueType>> AddIssue( SnIssueType issueType ) => AddEntity( issueType );
        public Task<Either<Error, Unit>> UpdateIssue( SnIssueType issueType ) => UpdateEntity( issueType );
        public Task<Either<Error, Unit>> DeleteIssue( SnIssueType issueType ) => DeleteEntity( issueType );
        public Task<Either<Error, SnIssueType>> GetIssue( EntityId issueTypeId ) => GetEntity( issueTypeId );
        public Task<Either<Error, IEnumerable<SnIssueType>>> GetIssues() => GetEntities();
        public Task<Either<Error, IEnumerable<SnIssueType>>> GetIssues( SnProject forProject ) => 
            GetEntities( c => c.ProjectId.Equals( forProject.EntityId ));
    }
}
