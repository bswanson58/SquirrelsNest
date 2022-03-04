using System.Diagnostics;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.EfDb.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class DbUser : DbBase {
        public  string  Name { get; set; }
        public  string  LoginName { get; set; }
        public  string  Email { get; set; }

        protected DbUser() {
            Name = String.Empty;
            LoginName = String.Empty;
            Email = String.Empty;
        }

        public static DbUser From( SnUser user ) {
            return new DbUser {
                EntityId = user.EntityId,
                Id = String.IsNullOrWhiteSpace( user.DbId ) ? DbIdDefault() : DbIdCreate( user.DbId ),
                Name = user.Name,
                LoginName = user.LoginName,
                Email = user.Email
            };
        }

        public SnUser ToEntity() {
            return new SnUser( EntityId, Id.ToString(), LoginName, Name, Email );
        }
    }
}
