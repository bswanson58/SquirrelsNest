using AutoMapper;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Types;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ProjectProvider : EntityProvider<DbProject>, IProjectProvider {
        private readonly IMapper    mMapper;

        public ProjectProvider( IDatabaseProvider databaseProvider, IMapper mapper )
            : base( databaseProvider, DbCollectionNames.ProjectCollection ) {
            mMapper = mapper;
        }

        public Either<Error, Unit> AddProject( SnProject project ) {
            return InsertEntity( mMapper.Map<DbProject>( project ));
        }

        public Either<Error, Unit> UpdateProject( SnProject project ) {
            return UpdateEntity( mMapper.Map<DbProject>( project ));
        }

        public Either<Error, Unit> DeleteProject( SnProject project ) {
            return DeleteEntity( mMapper.Map<DbProject>( project ));
        }

        public Either<Error, SnProject> GetProject( IssueId byId ) {
            return ValidateString( byId )
                .Bind( _ => FindEntity( LiteDB.Query.EQ( nameof( DbProject.Name ), byId.Value )))
                    .Map( dbProject => mMapper.Map<SnProject>( dbProject ));
        }

        public Either<Error, IEnumerable<SnProject>> GetProjects() {
            return GetList()
                .Map( projectList => mMapper.Map<IEnumerable<SnProject>>( projectList.ToEnumerable()));
        }
    }
}
