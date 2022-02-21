using System.Diagnostics;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("User = {" + nameof( LoginName ) + "}")]
    public class SnUser : EntityBase {
        public  string  Name { get; }
        public  string  LoginName { get; }

        public SnUser( string entityId, string dbId, string loginName, string displayName ) :
            base( entityId, dbId ){
            Name = displayName;
            LoginName = loginName;
        }

        public SnUser( string loginName ) :
            base( String.Empty ) {
            LoginName = loginName;
            Name = String.Empty;
        }

        public SnUser With( string ?  displayName ) {
            return new SnUser( EntityId, DbId, LoginName, displayName ?? Name );
        }
    }
}
