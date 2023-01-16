using System;
using System.Collections.Generic;
using System.Diagnostics;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( DisplayName ) + "}")]
    internal class TrUser : TrBase {
        public  string  DisplayName { get; set; }
        public  string  LoginName { get; set; }
        public  string  Email { get; set; }

        public TrUser() {
            DisplayName = String.Empty;
            LoginName = String.Empty;
            Email = String.Empty;
        }

        public static TrUser From( SnUser user ) {
            return new TrUser {
                EntityId = user.EntityId,
                DisplayName = user.DisplayName,
                LoginName = user.LoginName,
                Email = user.Email
            };
        }

        public SnUser ToEntity() {
            return new SnUser( EntityId, LoginName, DisplayName, Email, new List<string>());
        }

        private static TrUser ? mDefaultUser;

        public static TrUser Default =>
            mDefaultUser ??= new TrUser {
                EntityId = EntityIdentifier.Default,
                DisplayName = String.Empty,
                LoginName = String.Empty,
                Email = String.Empty
            } ;
    }
}
