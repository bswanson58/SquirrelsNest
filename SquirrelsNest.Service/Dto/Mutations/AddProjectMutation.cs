using System;
using System.Collections.Generic;
using LanguageExt.Common;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AddProjectInput {
        public  string      Title { get; set; }
        public  string      Description { get; set; }
        public  string      IssuePrefix { get; set; }
        public  string      ProjectTemplate { get; set; }

        public AddProjectInput() {
            Title = String.Empty;
            Description = String.Empty;
            IssuePrefix = String.Empty;
            ProjectTemplate = String.Empty;
        }
    }

    public class AddProjectPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClProject ?           Project { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public AddProjectPayload( ClProject project ) {
            Errors = new List<MutationError>();
            Project = project;
        }

        public AddProjectPayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public AddProjectPayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
