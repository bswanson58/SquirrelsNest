using System;
using System.Collections.Generic;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AddUserInput {
        public  string      Name { get; set; }
        public  string      LoginName { get; set; }
        public  string      Email { get; set; }
        public  string      Password { get; set; }

        public AddUserInput() {
            Name = String.Empty;
            LoginName = String.Empty;
            Email = String.Empty;
            Password = String.Empty;
        }
    }

    public class AddUserPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClUser ?            User { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public AddUserPayload( ClUser user ) {
            Errors = new List<MutationError>();
            User = user;
        }

        public AddUserPayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public AddUserPayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
