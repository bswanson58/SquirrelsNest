using System.Diagnostics;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class TrUser : TrBase {
        public  string  Name { get; set; }
        public  string  LoginName { get; set; }
        public  string  Email { get; set; }

        protected TrUser() {
            Name = String.Empty;
            LoginName = String.Empty;
            Email = String.Empty;
        }

        public static TrUser From( SnUser user ) {
            return new TrUser {
                EntityId = user.EntityId,
                Name = user.Name,
                LoginName = user.LoginName,
                Email = user.Email
            };
        }

        public SnUser ToEntity() {
            return new SnUser( EntityId, String.Empty, LoginName, Name, Email );
        }
    }
}
