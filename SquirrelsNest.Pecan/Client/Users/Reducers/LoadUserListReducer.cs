using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Users.Actions;
using SquirrelsNest.Pecan.Client.Users.Store;

namespace SquirrelsNest.Pecan.Client.Users.Reducers {
    // ReSharper disable once UnusedType.Global
    public class LoadUserListReducer {
        [ReducerMethod( typeof( GetUsersAction ))]
        public static UserState GetUserList( UserState state ) =>
            new ( true, String.Empty, state.Users );

        [ReducerMethod]
        public static UserState GetUsersSuccess( UserState _, GetUsersSuccessAction action ) =>
            new ( false, String.Empty, action.Users );

        [ReducerMethod]
        public static UserState GetUsersFailure( UserState state, GetUsersFailureAction action ) =>
            new ( false, action.Message, state.Users );
    }
}
