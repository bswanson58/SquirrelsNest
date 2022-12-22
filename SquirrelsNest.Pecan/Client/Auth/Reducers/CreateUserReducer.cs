using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Auth.Store;

namespace SquirrelsNest.Pecan.Client.Auth.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class CreateUserReducer {
        [ReducerMethod( typeof( CreateUserAction ))]
        public static AuthState ReduceAddProjectAction( AuthState state ) => state;

        [ReducerMethod( typeof( CreateUserSubmitAction ))]
        public static AuthState CreateUserSubmitReducer( AuthState state ) =>
            new ( true, String.Empty, state.UserToken, state.RefreshToken, state.TokenExpiration );

        [ReducerMethod( typeof( CreateUserSuccessAction ))]
        public static AuthState CreateUserSuccessReducer( AuthState state ) =>
            new ( false, String.Empty, state.UserToken, state.RefreshToken, state.TokenExpiration );

        [ReducerMethod]
        public static AuthState CreateUserFailureReducer( AuthState state, CreateUserFailureAction action ) =>
            new ( false, action.Message, state.UserToken, state.RefreshToken, state.TokenExpiration );
    }
}
