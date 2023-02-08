using System;
using System.Collections.Generic;
using LanguageExt.Common;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EditUserRolesInput {
        public  string          Email { get; set; }
        public  List<ClClaim>   Claims { get; set; }

        public EditUserRolesInput() {
            Email = String.Empty;
            Claims = new List<ClClaim>();
        }
    }

    public class EditUserRolesPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClUser ?            User { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public EditUserRolesPayload( ClUser user ) {
            Errors = new List<MutationError>();
            User = user;
        }

        public EditUserRolesPayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public EditUserRolesPayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
