using System;
using System.Collections.Generic;
using LanguageExt.Common;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AddProjectDetailInput {
        public  string                  ProjectId { get; set; }
        public  List<ClIssueType>       IssueTypes { get; set; }
        public  List<ClComponent>       Components { get; set; }
        public  List<ClWorkflowState>   States { get; set; }

        public AddProjectDetailInput() {
            ProjectId = String.Empty;
            IssueTypes = new List<ClIssueType>();
            Components = new List<ClComponent>();
            States = new List<ClWorkflowState>();
        }
    }

    public class AddProjectDetailPayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClProject ?         Project { get; set; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public AddProjectDetailPayload( ClProject project ) {
            Project = project;
            Errors = new List<MutationError>();
        }

        public AddProjectDetailPayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public AddProjectDetailPayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
