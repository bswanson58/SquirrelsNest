using System;
using FluentValidation;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.Issues {
    public class CreateIssueRequest {
        public  string      Title { get; set; }
        public  string      Description {  get; set; }
        public  string      ProjectId { get; }
        public  string      ComponentId { get; set; }
        public  string      IssueTypeId { get; set; }
        public  string      WorkflowStateId {  get; set; }

        public const string Route = $"{Routes.BaseRoute}/createIssue";

        [JsonConstructor]
        public CreateIssueRequest( string title, string description, string projectId, string componentId, string issueTypeId,
                                   string workflowStateId ) {
            Title = title;
            Description = description;
            ProjectId = projectId;
            ComponentId = componentId;
            IssueTypeId = issueTypeId;
            WorkflowStateId = workflowStateId;
        }

        public CreateIssueRequest( SnProject forProject ) {
            Title = String.Empty;
            Description = String.Empty;
            ProjectId = forProject.EntityId;
            ComponentId = SnComponent.Default.EntityId;
            IssueTypeId = SnIssueType.Default.EntityId;
            WorkflowStateId = SnWorkflowState.Default.EntityId;
        }
    }

    public class CreateIssueResponse : BaseResponse {
        public  SnCompositeIssue ?      Issue { get; }
        public  SnCompositeProject ?    Project { get; }

        [JsonConstructor]
        public CreateIssueResponse( bool succeeded, string message, SnCompositeIssue issue, SnCompositeProject project ) :
            base( succeeded, message ) {
            Issue = issue;
            Project = project;
        }

        public CreateIssueResponse( SnCompositeIssue issue, SnCompositeProject project ) {
            Issue = issue;
            Project = project;
        }

        public CreateIssueResponse( Exception ex ) :
            base( ex ) {
            Issue = null;
            Project = null;
        }

        public CreateIssueResponse( string message ) :
            base( false, message ) {
            Issue = null;
            Project = null;
        }

        public CreateIssueResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Issue = null;
            Project = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class CreateIssueRequestValidator : AbstractValidator<CreateIssueRequest> {
        public CreateIssueRequestValidator() {
            RuleFor( p => p.Title )
                .NotEmpty()
                .WithMessage( "A title for the issue is required." );

            RuleFor( p => p.Description )
                .NotNull()
                .WithMessage( "The issue description may not be null." );

            RuleFor( p => p.ProjectId )
                .NotEmpty()
                .WithMessage( "An project for the issue is required." );
        }
    }
}
