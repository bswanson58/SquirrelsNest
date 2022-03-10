using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.EfDb.Context;
using SquirrelsNest.EfDb.Dto;

namespace SquirrelsNest.EfDb.Providers {
    internal class UserProvider : EntityProvider<SnUser, DbUser>, IDbUserProvider {
        public UserProvider( IContextFactory contextFactory )
            : base( contextFactory ) { }

        protected override SnUser ConvertTo( DbUser user ) => user.ToEntity();
        protected override DbUser ConvertFrom( SnUser user ) => DbUser.From( user );

        public Task<Either<Error, SnUser>> AddUser( SnUser user ) => AddEntity( user );
        public Task<Either<Error, Unit>> UpdateUser( SnUser user ) => UpdateEntity( user );
        public Task<Either<Error, Unit>> DeleteUser( SnUser user ) => DeleteEntity( user );
        public Task<Either<Error, SnUser>> GetUser( EntityId userId ) => GetEntity( userId );
        public Task<Either<Error, IEnumerable<SnUser>>> GetUsers() => GetEntities();
    }
}
