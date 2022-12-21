using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Auth.Store;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Client.Auth.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class LoginUserReducer {
        [ReducerMethod( typeof( LoginUserAction ))]
        public static AuthState ReduceAddProjectAction( AuthState state ) => state;

        [ReducerMethod( typeof( LoginUserSubmitAction ))]
        public static AuthState CreateUserSubmitReducer( AuthState state ) =>
            new ( true, String.Empty, state.UserToken, state.TokenExpiration );

        [ReducerMethod]
        public static AuthState CreateUserSuccessReducer( AuthState state, LoginUserSuccessAction action ) =>
            new ( false, String.Empty, action.UserResponse.Token, action.UserResponse.Expiration );

        [ReducerMethod]
        public static AuthState LoginUserFailureReducer( AuthState state, LoginUserFailureAction action ) =>
            new ( false, action.Message, String.Empty, DateTimeProvider.Instance.CurrentDateTime );
    }
}
