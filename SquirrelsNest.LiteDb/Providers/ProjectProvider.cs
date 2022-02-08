﻿using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ProjectProvider : EntityProvider<DbProject>, IProjectProvider {
        public ProjectProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.ProjectCollection ) {
        }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbProject>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnProject> AddProject( SnProject project ) {
            return InsertEntity( DbProject.From( project ))
                .Map( dbProject => dbProject.ToEntity());
        }

        public Either<Error, Unit> UpdateProject( SnProject project ) {
            return UpdateEntity( DbProject.From( project ));
        }

        public Either<Error, Unit> DeleteProject( SnProject project ) {
            return DeleteEntity( DbProject.From( project ));
        }

        public Either<Error, SnProject> GetProject( EntityId projectId ) {
            return ValidateString( projectId )
                .Bind( _ => FindEntity( LiteDB.Query.EQ( nameof( DbProject.EntityId ), projectId.Value )))
                    .Map( dbProject => dbProject.ToEntity());
        }

        public Either<Error, IEnumerable<SnProject>> GetProjects() {
            return GetList()
                .Map( projectList => projectList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}
