using System;
using Microsoft.AspNetCore.Identity;

namespace SquirrelsNest.Service.Dto {
    public class DbUser {
        public  string  Id { get; set; }
        public  string  Email { get; set; }

        public DbUser() {
            Id = String.Empty;
            Email = String.Empty;
        }

        public DbUser( IdentityUser fromUser ) {
            Id = fromUser.Id;
            Email = fromUser.Email;
        }
    }
}
