using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Core.Database {
    internal class UserProvider : BaseDeleteProvider, IUserProvider {
        private readonly IDbUserProvider        mUserProvider;
        private readonly IDbUserDataProvider    mUserDataProvider;
        private readonly IDbAssociationProvider mAssociationProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mUserProvider.OnEntitySourceChange;

        public UserProvider( IDbIssueProvider issueProvider, IDbUserProvider userProvider, IDbUserDataProvider userDataProvider,
                             IDbAssociationProvider associationProvider ) :
            base( issueProvider ) {
            mUserProvider = userProvider;
            mUserDataProvider = userDataProvider;
            mAssociationProvider = associationProvider;
        }

        public Task<Either<Error, SnUser>> AddUser( SnUser user ) => mUserProvider.AddUser( user );
        public Task<Either<Error, Unit>> UpdateUser( SnUser user ) => mUserProvider.UpdateUser( user );
        public Task<Either<Error, SnUser>> GetUser( EntityId userId ) => mUserProvider.GetUser( userId );
        public async Task<Either<Error, SnUser>> GetUser( string email ) {
            return ( await GetUsers())
                .Map( list => list.Where( u => u.Email.Equals( email )))
                .Map( u => u.FirstOrDefault( SnUser.Default ))
                .Bind( ConvertDefaultUser );
        }

        private static Either<Error, SnUser> ConvertDefaultUser( SnUser user ) =>
            user.EntityId.Equals( SnUser.Default.EntityId ) ? 
                Error.New( "A user could not be located" ) :
                user;

        public Task<Either<Error, IEnumerable<SnUser>>> GetUsers() => mUserProvider.GetUsers();

        private async Task<Either<Error, Unit>> DeleteUserData( SnUser forUser ) {
            var data = await mUserDataProvider.GetData( forUser );

            if( data.IsLeft ) {
                return data.Map( _ => Unit.Default );
            }

            return await data.BindAsync( async list => {
                foreach( var d in list ) {
                    var result = await mUserDataProvider.DeleteData( d );

                    if( result.IsLeft ) {
                        return result;
                    }
                }

                return Unit.Default;
            });
        }

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
                .BindAsync( _ => DeleteUserData( user ))
                .BindAsync( _ => mUserProvider.DeleteUser( user )).ConfigureAwait( false );
        }

        public async Task<Either<Error, SnAssociation>> AddAssociation( SnUser forUser, SnProject toProject ) {
            var association = new SnAssociation( forUser.EntityId, toProject.EntityId );

            return await mAssociationProvider.AddAssociation( association ).ConfigureAwait( false );
        }

        private async Task<Either<Error, Unit>> DeleteAssociations( IEnumerable<SnAssociation> deleteList ) {
            foreach( var association in deleteList ) {
                var result = await mAssociationProvider.DeleteAssociation( association ).ConfigureAwait( false );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        public async Task<Either<Error, Unit>> DeleteAssociation( SnUser fromUser, SnProject fromProject ) {
            var toDelete = ( await mAssociationProvider.GetAssociations( fromUser ))
                .Map( list => from a in list where a.AssociationId.Equals( fromProject.EntityId ) select a );

            return await toDelete.BindAsync( DeleteAssociations ).ConfigureAwait( false );
        }

        public override void Dispose() {
            mUserProvider.Dispose();

            base.Dispose();
        }
    }
}
