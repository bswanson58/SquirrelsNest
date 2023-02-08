using System;
using System.Collections.Generic;
using LanguageExt.Common;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto.Mutations {
    public class DeleteProjectInput {
        public  string      ProjectId { get; set; }

        public DeleteProjectInput() {
            ProjectId = String.Empty;
        }
    }

    public class DeleteProjectPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  string      ProjectId { get; set; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public DeleteProjectPayload( EntityId projectId ) {
            ProjectId = projectId;
            Errors = new List<MutationError>();
        }

        public DeleteProjectPayload( Error error ) {
            ProjectId = String.Empty;
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public DeleteProjectPayload( string error ) {
            ProjectId = String.Empty;
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
