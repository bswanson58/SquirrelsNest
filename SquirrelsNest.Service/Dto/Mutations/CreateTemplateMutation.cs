using System.Collections.Generic;
using System;
using LanguageExt.Common;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CreateTemplateInput {
        public string ProjectId { get; }
        public string Name { get; }
        public string Description { get; }

        public CreateTemplateInput( string projectId, string name, string description ) {
            ProjectId = projectId;
            Name = name;
            Description = description;
        }
    }

    public class CreateTemplatePayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  Boolean             Succeeded { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public CreateTemplatePayload() {
            Succeeded = true;
            Errors = new List<MutationError>();
        }

        public CreateTemplatePayload( Error error ) {
            Succeeded = false;
            Errors = new List<MutationError>{ new MutationError( error ) };
        }
    }
}
