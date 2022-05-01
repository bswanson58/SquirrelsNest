using System;
using System.Collections.Generic;
using LanguageExt.Common;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EditIssueInput {
        public  string      IssueId { get; set; }
        public  string      Title { get; set; }
        public  string      Description { get; set; }
        public  string      IssueTypeId {  get; set; }
        public  string      ComponentId { get; set; } 
        public  string      ReleaseId { get; set; }
        public  string      WorkflowStateId { get; set; }
        public  string      AssignedToId { get; set; }

        public EditIssueInput() {
            IssueId = String.Empty;
            Title = String.Empty;
            Description = String.Empty;
            IssueTypeId = String.Empty;
            ComponentId = String.Empty;
            ReleaseId = String.Empty;
            WorkflowStateId = String.Empty;
            AssignedToId = String.Empty;
        }
    }

    public class EditIssuePayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClIssue ?           Issue { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public EditIssuePayload( CompositeIssue fromIssue ) {
            Errors = new List<MutationError>();
            Issue = IssueExtensions.ToCl( fromIssue );
        }

        public EditIssuePayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public EditIssuePayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
