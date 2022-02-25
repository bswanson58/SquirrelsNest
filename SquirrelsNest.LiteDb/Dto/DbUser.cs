using System.Diagnostics;
using LiteDB;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.LiteDb.Dto {
    [DebuggerDisplay("{" + nameof( Version ) + "}")]
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
                Id = String.IsNullOrWhiteSpace( user.DbId ) ? ObjectId.NewObjectId() : new ObjectId( user.DbId ),
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
