using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class ReleaseProvider : EntityProvider<SnRelease, DbRelease>, IDbReleaseProvider {
        public ReleaseProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnRelease ConvertTo( DbRelease release ) => release.ToEntity();
        protected override DbRelease ConvertFrom( SnRelease release ) => DbRelease.From( release );

        public Task<Either<Error, SnRelease>> AddRelease( SnRelease release ) => AddEntity( release );
        public Task<Either<Error, Unit>> UpdateRelease( SnRelease release ) => UpdateEntity( release );
        public Task<Either<Error, Unit>> DeleteRelease( SnRelease release ) => DeleteEntity( release );
        public Task<Either<Error, SnRelease>> GetRelease( EntityId releaseId ) => GetEntity( releaseId );
        public Task<Either<Error, IEnumerable<SnRelease>>> GetReleases() => GetEntities();
        public Task<Either<Error, IEnumerable<SnRelease>>> GetReleases( SnProject forProject ) => 
            GetEntities( c => c.ProjectId.Equals( forProject.EntityId ));
    }
}
