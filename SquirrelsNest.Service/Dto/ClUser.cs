using System;
using System.Collections.Generic;
using System.Security.Claims;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto {
    public class ClClaim {
        public  string  Type { get; set; }
        public  string  Value { get; set; }

        public ClClaim() {
            Type = String.Empty;
            Value = String.Empty;
        }
    }

    public class ClUser : ClBase {
        public string           Name { get; }
        public string           LoginName { get; }
        public string           Email { get; }
        public List<ClClaim>    Claims { get; set; }

        public ClUser( string id, string displayName, string loginName, string email, IEnumerable<ClClaim> claims ) :
            base( id ) {
            Name = displayName;
            LoginName = loginName;
            Email = email;
            Claims = new List<ClClaim>( claims );
        }

        private static ClUser ? mDefaultUser;

        public static ClUser Default =>
            mDefaultUser ??= new ClUser( EntityId.Default.Value, "Unspecified", "Unspecified", String.Empty, new List<ClClaim>());
    }

    public static class UserExtensions {
        private static ClClaim NewClClaim( Claim fromClaim ) {
            return new ClClaim() { 
                Type = fromClaim.Type,
                Value = fromClaim.Value
            };
        }

        public static ClUser ToCl( this SnUser user ) {
            return new ClUser( user.EntityId, user.Name, user.LoginName, user.Email, new List<ClClaim>());
        }

        public static ClUser With( this ClUser user, IEnumerable<Claim> claims ) {
            foreach( var claim in claims ) {
                user.Claims.Add( NewClClaim( claim ));
            }

            return user;
        }
    }

}
