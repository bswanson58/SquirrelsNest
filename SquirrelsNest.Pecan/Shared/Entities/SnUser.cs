using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Entities {
    [DebuggerDisplay("User: {" + nameof( LoginName ) + "}")]
    public class SnUser : EntityBase {
        public  string          DisplayName { get; }
        public  string          LoginName { get; }
        public  string          Email { get; }
        public  IList<string>   RoleClaims { get; }

        [JsonConstructor]
        public SnUser( string entityId, string loginName, string displayName, string email, IList<string> roleClaims ) :
            base( entityId ){
            DisplayName = displayName;
            LoginName = loginName;
            Email = email;
            RoleClaims = roleClaims;
        }

        public SnUser( string loginName, string email ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( loginName )) throw new ArgumentException( "User login name cannot be empty", nameof( loginName ));
            if( String.IsNullOrWhiteSpace( email )) throw new ArgumentException( "User email cannot be empty", nameof( email ));

            LoginName = loginName;
            DisplayName = loginName;
            Email = email;
            RoleClaims = new List<string>();
        }

        public SnUser With( string ?  displayName, string ? email = null ) {
            return new SnUser( EntityId, LoginName, displayName ?? DisplayName, email ?? Email, RoleClaims );
        }

        private static SnUser ? mDefaultUser;

        public static SnUser Default =>
            mDefaultUser ??= 
                new SnUser( EntityIdentifier.Default, "Unspecified", "Unspecified", "Unspecified", new List<string>());
    }
}
