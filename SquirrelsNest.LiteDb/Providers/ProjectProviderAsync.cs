using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ProjectProviderAsync : ProjectProvider, IProjectProvider {
        public ProjectProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnProject>> AddProject( SnProject project ) {
            return Task.Run(()=> base.AddProject( project ));
        }

        public new Task<Either<Error, Unit>> UpdateProject( SnProject project ) {
            return Task.Run(() => base.UpdateProject( project ));
        }

        public new Task<Either<Error, Unit>> DeleteProject( SnProject project ) {
            return Task.Run(() => base.DeleteProject( project ));
        }

        public new Task<Either<Error, SnProject>> GetProject( EntityId projectId ) {
            return Task.Run(() => base.GetProject( projectId ));
        }

        public new Task<Either<Error, IEnumerable<SnProject>>> GetProjects() {
            return Task.Run(() => base.GetProjects());
        }
    }
}
