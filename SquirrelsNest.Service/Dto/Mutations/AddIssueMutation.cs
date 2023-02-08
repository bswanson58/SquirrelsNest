using System;
using System.Collections.Generic;
using LanguageExt.Common;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AddIssueInput {
        public  string      ProjectId { get; set; }
        public  string      Title { get; set; }
        public  string      Description { get; set; }
        public  string      IssueTypeId { get; set; }
        public  string      ComponentId {  get; set; }
        public  string      WorkflowId {  get; set; }

        public AddIssueInput() {
            ProjectId = String.Empty;
            Title = String.Empty;
            Description = String.Empty;
            IssueTypeId = String.Empty;
            ComponentId = String.Empty;
            WorkflowId = String.Empty;
        }
    }

    public class AddIssuePayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClIssue ?           Issue { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public AddIssuePayload( CompositeIssue fromIssue ) {
            Errors = new List<MutationError>();
            Issue = IssueExtensions.ToCl( fromIssue );
        }

        public AddIssuePayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public AddIssuePayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
