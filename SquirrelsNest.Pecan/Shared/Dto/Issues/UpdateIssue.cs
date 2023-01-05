using System;
using FluentValidation;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.Issues {
    public class UpdateIssueRequest {
        public  string      IssueId { get; set; }
        public  string      Title { get; set; }
        public  string      Description {  get; set; }
        public  string      ProjectId { get; }
        public  string      ComponentId { get; set; }
        public  string      IssueTypeId { get; set; }
        public  string      WorkflowStateId {  get; set; }

        public const string Route = $"{Routes.BaseRoute}/updateIssue";

        [JsonConstructor]
        public UpdateIssueRequest( string issueId, string title, string description, string projectId, string componentId,
                                   string issueTypeId, string workflowStateId ) {
            IssueId = issueId;
            Title = title;
            Description = description;
            ProjectId = projectId;
            ComponentId = componentId;
            IssueTypeId = issueTypeId;
            WorkflowStateId = workflowStateId;
        }

        public UpdateIssueRequest( SnProject forProject, SnIssue forIssue ) {
            IssueId = forIssue.EntityId;
            Title = String.Empty;
            Description = String.Empty;
            ProjectId = forProject.EntityId;
            ComponentId = SnComponent.Default.EntityId;
            IssueTypeId = SnIssueType.Default.EntityId;
            WorkflowStateId = SnWorkflowState.Default.EntityId;
        }
    }

    public class UpdateIssueResponse : BaseResponse {
        public  SnCompositeIssue ?  Issue { get; }

        [JsonConstructor]
        public UpdateIssueResponse( bool succeeded, string message, SnCompositeIssue issue ) :
            base( succeeded, message ) {
            Issue = issue;
        }

        public UpdateIssueResponse( SnCompositeIssue issue ) {
            Issue = issue;
        }

        public UpdateIssueResponse( Exception ex ) :
            base( ex ) {
            Issue = null;
        }

        public UpdateIssueResponse( string message ) :
            base( false, message ) {
            Issue = null;
        }

        public UpdateIssueResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Issue = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class UpdateIssueRequestValidator : AbstractValidator<UpdateIssueRequest> {
        public UpdateIssueRequestValidator() {
            RuleFor( p => p.IssueId )
                .NotEmpty()
                .WithMessage( "An Issue ID is required." );

            RuleFor( p => p.ProjectId )
                .NotEmpty()
                .WithMessage( "A Project ID for the issue is required." );

            RuleFor( p => p.Title )
                .NotEmpty()
                .WithMessage( "A title for the issue is required." );

            RuleFor( p => p.Description )
                .NotNull()
                .WithMessage( "The issue description may not be null." );
        }
    }
}
