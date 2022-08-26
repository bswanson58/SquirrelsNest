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
        public ClUser ?             User { get; }
        public List<MutationError>  Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public DeleteUserPayload( ClUser user ) {
            User = user;
            Errors = new List<MutationError>();
        }

        public DeleteUserPayload( Error error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }

        public DeleteUserPayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
