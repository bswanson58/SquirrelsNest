using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class ProjectProvider : EntityProvider<SnProject, DbProject>, IProjectProvider {
        public ProjectProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnProject ConvertTo( DbProject project ) => project.ToEntity();
        protected override DbProject ConvertFrom( SnProject project ) => DbProject.From( project );

        public Task<Either<Error, SnProject>> AddProject( SnProject project ) => AddEntity( project );
        public Task<Either<Error, Unit>> UpdateProject( SnProject project ) => UpdateEntity( project );
        public Task<Either<Error, Unit>> DeleteProject( SnProject project ) => DeleteEntity( project );
        public Task<Either<Error, SnProject>> GetProject( EntityId projectId ) => GetEntity( projectId );
        public Task<Either<Error, IEnumerable<SnProject>>> GetProjects() => GetEntities();
    }
}
