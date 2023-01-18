using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Users.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Users.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class UpdateUserRolesReducer {
        [ReducerMethod]
        public static UserState ChangeUserRoles( UserState state, ChangeUserRolesSuccess action ) {
            var users = new List<SnUser>();

            users.AddRange( state.Users.Select( u => u.EntityId.Equals( action.User.EntityId ) ? action.User : u ));

            return new( false, String.Empty, users );
        }
    }
}
