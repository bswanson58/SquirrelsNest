using System;
using FluentValidation;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.Issues {
    public class DeleteIssueRequest {
        public  SnCompositeIssue    Issue { get; }

        public const string Route = $"{Routes.BaseRoute}/deleteIssue";

        [JsonConstructor]
        public DeleteIssueRequest( SnCompositeIssue issue ) {
            Issue = issue;
        }
    }

    public class DeleteIssueResponse : BaseResponse {
        public  SnCompositeIssue ?  Issue { get; }

        [JsonConstructor]
        public DeleteIssueResponse( bool succeeded, string message, SnCompositeIssue issue ) :
            base( succeeded, message ) {
            Issue = issue;
        }

        public DeleteIssueResponse( SnCompositeIssue issue ) {
            Issue = issue;
        }

        public DeleteIssueResponse( Exception ex ) :
            base( ex ) {
            Issue = null;
        }

        public DeleteIssueResponse( string message ) :
            base( false, message ) {
            Issue = null;
        }

        public DeleteIssueResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Issue = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class DeleteIssueRequestValidator : AbstractValidator<DeleteIssueRequest> {
        public DeleteIssueRequestValidator() {
            RuleFor( p => p.Issue )
                .NotNull()
                .WithMessage( "An Issue is required." );

            RuleFor( p => p.Issue.EntityId )
                .NotEmpty()
                .WithMessage( "A valid issue is required." );

            RuleFor( p => p.Issue.ProjectId )
                .NotEmpty()
                .WithMessage( "A valid issue is required." );
        }
    }
}
