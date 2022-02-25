using System.Diagnostics;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("User: {" + nameof( LoginName ) + "}")]
    public class SnUser : EntityBase {
        public  string  Name { get; }
        public  string  LoginName { get; }
        public  string  Email { get; }

        public SnUser( string entityId, string dbId, string loginName, string displayName, string email ) :
            base( entityId, dbId ){
            Name = displayName;
            LoginName = loginName;
            Email = email;
        }

        public SnUser( string loginName, string email ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( loginName )) throw new ArgumentException( "User login name cannot be empty", nameof( loginName ));
            if( String.IsNullOrWhiteSpace( email )) throw new ArgumentException( "User email cannot be empty", nameof( email ));

            LoginName = loginName;
            Name = loginName;
            Email = email;
        }

        public SnUser With( string ?  displayName, string ? email = null ) {
            return new SnUser( EntityId, DbId, LoginName, displayName ?? Name, email ?? Email );
        }

        private static SnUser ? mDefaultUser;

        public static SnUser Default =>
            mDefaultUser ??= new SnUser( Values.EntityId.Default, String.Empty, "Unspecified", "Unspecified", "Unspecified" );
    }
}
