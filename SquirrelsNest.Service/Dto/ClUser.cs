using System;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto {
    public class ClUser : ClBase {
        public string Name { get; }
        public string LoginName { get; }
        public string Email { get; }

        public ClUser( string id, string displayName, string loginName, string email ) :
            base( id ) {
            Name = displayName;
            LoginName = loginName;
            Email = email;
        }

        private static ClUser ? mDefaultUser;

        public static ClUser Default =>
            mDefaultUser ??= new ClUser( EntityId.Default.Value, "Unspecified", "Unspecified", String.Empty );
    }

    public static class UserExtensions {
        public static ClUser From( this SnUser user ) {
            return new ClUser( user.EntityId, user.Name, user.LoginName, user.Email );
        }
    }

}
