using System;
using System.Collections.Generic;
using LanguageExt.Common;

namespace SquirrelsNest.Service.Dto.Mutations {
    public class UpdateProjectInput {
        public  string      ProjectId {  get; set; }
        public  string      Title { get; set; }
        public  string      Description { get; set; }
        public  string      IssuePrefix { get; set; }

        public UpdateProjectInput() {
            ProjectId = String.Empty;
            Title = String.Empty;
            Description = String.Empty;
            IssuePrefix = String.Empty;
        }
    }

    public class UpdateProjectPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClProject ?         Project { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public UpdateProjectPayload( ClProject project ) {
            Errors = new List<MutationError>();
            Project = project;
        }

        public UpdateProjectPayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public UpdateProjectPayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
