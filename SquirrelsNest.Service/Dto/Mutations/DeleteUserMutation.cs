using System;
using System.Collections.Generic;
using LanguageExt.Common;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DeleteUserInput {
        public string Email { get; set; }

        public DeleteUserInput() {
            Email = String.Empty;
        }
    }

    public class DeleteUserPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string Email { get; set; }
        public List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public DeleteUserPayload( ClUser user ) {
            Email = user.Email;
            Errors = new List<MutationError>();
        }

        public DeleteUserPayload( Error error ) {
            Email = String.Empty;
            Errors = new List<MutationError> { new MutationError( error ) };
        }

        public DeleteUserPayload( string error ) {
            Email = String.Empty;
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
