using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Core.Database {
    internal class UserProvider : BaseDeleteProvider, IUserProvider {
        private readonly IDbUserProvider    mUserProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mUserProvider.OnEntitySourceChange;

        public UserProvider( IDbIssueProvider issueProvider, IDbUserProvider userProvider ) :
            base( issueProvider ) {
            mUserProvider = userProvider;
        }

        public Task<Either<Error, SnUser>> AddUser( SnUser user ) => mUserProvider.AddUser( user );
        public Task<Either<Error, Unit>> UpdateUser( SnUser user ) => mUserProvider.UpdateUser( user );
        public Task<Either<Error, SnUser>> GetUser( EntityId userId ) => mUserProvider.GetUser( userId );
        public Task<Either<Error, IEnumerable<SnUser>>> GetUsers() => mUserProvider.GetUsers();

        public async Task<Either<Error, Unit>> DeleteUser( SnUser user ) {
            var assignedBy = ( await mIssueProvider.GetIssues().ConfigureAwait( false ))
                .Map( list => from i in list where i.AssignedToId.Equals( user.EntityId ) select i )
                .Map( list => from i in list select i.With( assignedTo: SnUser.Default.EntityId ));

            var enteredBy = ( await mIssueProvider.GetIssues().ConfigureAwait( false ))
                .Map( list => from i in list where i.EnteredById.Equals( user.EntityId ) select i )
                .Map( list => from i in list select i.With( enteredBy: SnUser.Default.EntityId ));

            return await assignedBy
                .BindAsync( UpdateIssues )
                .BindAsync( _ => enteredBy.BindAsync( UpdateIssues ))
                .BindAsync( _ => mUserProvider.DeleteUser( user )).ConfigureAwait( false );
        }

        public override void Dispose() {
            mUserProvider.Dispose();

            base.Dispose();
        }
    }
}
