using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ProjectProvider : BaseProvider<SnProject, DbProject> {
        private static SnProject ConvertTo( DbProject project ) => project.ToEntity();
        private static DbProject ConvertFrom( SnProject project ) => DbProject.From( project );

        public ProjectProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.ProjectCollection, ConvertFrom, ConvertTo ) {
        }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbProject>().Id( e => e.Id );
            
            return base.InitializeDatabase( db );
        }

        public Either<Error, SnProject> AddProject( SnProject project ) => Add( project );
        public Either<Error, Unit> UpdateProject( SnProject project ) => Update( project );
        public Either<Error, Unit> DeleteProject( SnProject project ) => Delete( project );
        public Either<Error, SnProject> GetProject( EntityId projectId ) => Get( projectId );
        public Either<Error, IEnumerable<SnProject>> GetProjects() => GetEnumerable();
    }
}
