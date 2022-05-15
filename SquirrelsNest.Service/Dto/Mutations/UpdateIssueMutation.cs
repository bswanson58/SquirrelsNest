using System;
using System.Collections.Generic;
using LanguageExt.Common;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Service.Dto.Mutations {
    public enum IssueUpdatePath {
        Unknown = 0,
        Title = 1,
        Description = 2,
        IssueTypeId = 3,
        ComponentId = 4,
        ReleaseId = 5,
        WorkflowStateId = 6,
        AssignedToId = 7
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class UpdateOperation {
        public  IssueUpdatePath     Path {  get; set; }
        public  string              Value { get; set; }

        public UpdateOperation() {
            Path = IssueUpdatePath.Unknown;
            Value = String.Empty;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class UpdateIssueInput {
        public  string                  IssueId { get; set; }
        public  List<UpdateOperation>   Operations { get; set; }

        public UpdateIssueInput() {
            IssueId = String.Empty;
            Operations = new List<UpdateOperation>();
        }
    }

    public class UpdateIssuePayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  ClIssue ?           Issue { get; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public UpdateIssuePayload( CompositeIssue fromIssue ) {
            Errors = new List<MutationError>();
            Issue = IssueExtensions.ToCl( fromIssue );
        }

        public UpdateIssuePayload( Error error ) {
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public UpdateIssuePayload( string error ) {
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }
}
