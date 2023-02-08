using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Users.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory")]
    public class UserState : RootState {
        public IReadOnlyList<SnUser>    Users { get; }

        public UserState( bool callInProgress, string callMessage, 
            IEnumerable<SnUser> userList ) :
            base( callInProgress, callMessage ) {
            Users = new List<SnUser>( userList );
        }

        public static UserState Factory() => new ( false, string.Empty, Enumerable.Empty<SnUser>());
    }}
