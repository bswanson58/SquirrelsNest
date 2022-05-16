using System;
using System.Collections.Generic;
using LanguageExt.Common;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Service.Dto.Mutations {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DeleteIssueInput {
        public  string      IssueId { get; set; }

        public DeleteIssueInput() {
            IssueId = String.Empty;
        }
    }

    public class DeleteIssuePayload {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  string      IssueId { get; set; }
        public  List<MutationError> Errors { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public DeleteIssuePayload( EntityId issueId ) {
            IssueId = issueId;
            Errors = new List<MutationError>();
        }

        public DeleteIssuePayload( Error error ) {
            IssueId = String.Empty;
            Errors = new List<MutationError>{ new MutationError( error ) };
        }

        public DeleteIssuePayload( string error ) {
            IssueId = String.Empty;
            Errors = new List<MutationError> { new MutationError( error ) };
        }
    }

}
