using System;
using System.Collections.Generic;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EditUserPasswordInput {
        public  string      Email { get; set; }
        public  string      CurrentPassword { get; set; }
        public  string      NewPassword { get; set; }

        public EditUserPasswordInput() {
            Email = String.Empty;
            CurrentPassword = String.Empty;
            NewPassword = String.Empty;
        }
    }

    public class EditUserPasswordPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  string              Email { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public EditUserPasswordPayload( IdentityUser user ) {
            Errors = new List<MutationError>();
            Email = user.Email;
        }

        public EditUserPasswordPayload( Error error ) {
            Email = String.Empty;
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public EditUserPasswordPayload( string error ) {
            Email = String.Empty;
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
