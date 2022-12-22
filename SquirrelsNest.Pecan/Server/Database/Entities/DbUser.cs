﻿using System;
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

    /*
    public class DbUser : DbEntityBase<DbUser> {
        public  string  Name { get; set; }
        public  string  LoginName { get; set; }
        public  string  Email { get; set; }

        public DbUser() {
            Name = String.Empty;
            LoginName = String.Empty;
            Email = String.Empty;
        }

        public DbUser( SnUser user ) :
            base( user.EntityId ) {
            Name = user.Name;
            LoginName = user.LoginName;
            Email = user.Email;
        }

        public static DbUser From( SnUser user ) => new DbUser( user );

        public SnUser ToEntity() => new SnUser( EntityId, LoginName, Name, Email );

        public override void Update( DbUser from ) {
            Name = from.Name;
            LoginName = from.LoginName;
            Email = from.Email;
        }
    }
    */
}
