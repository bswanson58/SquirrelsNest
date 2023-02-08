using System;
using SquirrelsNest.Core.CompositeBuilders;
using System.Collections.Generic;
using LanguageExt.Common;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ModifyIssueInput {
        public string       IssueId { get; set; }
        public string       Title { get; set; }
        public string       Description { get; set; }
        public string       ComponentId { get; set; }
        public string       IssueTypeId { get; set; }
        public string       WorkflowStateId { get; set; }
        public string       ReleaseId { get; set; }
        public string       AssignedToId { get; set; }

        public ModifyIssueInput() {
            IssueId = String.Empty;
            Title = String.Empty;
            Description = String.Empty;
            ComponentId = String.Empty;
            IssueTypeId = String.Empty;
            WorkflowStateId = String.Empty;
            ReleaseId = String.Empty;
            AssignedToId = String.Empty;
        }
    }

    public class ModifyIssuePayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClIssue ?           Issue { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public ModifyIssuePayload( CompositeIssue fromIssue ) {
            Errors = new List<MutationError>();
            Issue = IssueExtensions.ToCl( fromIssue );
        }

        public ModifyIssuePayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public ModifyIssuePayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
