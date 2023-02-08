using System;
using Microsoft.AspNetCore.Identity;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbUser : IdentityUser {
        public  string      RefreshToken { get; set; }
        public  DateTime    RefreshTokenExpiration { get; set; }

        public DbUser() {
            RefreshToken = String.Empty;
            RefreshTokenExpiration = DateTimeProvider.Instance.CurrentDateTime;
        }
    }
}
