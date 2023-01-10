using System;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class EditProjectRequest {
        public  string      ProjectId { get; }
        public  string      Name { get; set; }
        public  string      Description { get; set; }
        public  string      IssuePrefix { get; set; }
        public  uint        NextIssueNumber { get; set; }

        public const string Route = $"{Routes.BaseRoute}/updateProject";

        [JsonConstructor]
        public EditProjectRequest( string projectId, string name, string description, string issuePrefix, uint nextIssueNumber ) {
            ProjectId = projectId;
            Name = name;
            Description = description;
            IssuePrefix = issuePrefix;
            NextIssueNumber = nextIssueNumber;
        }

        public EditProjectRequest( SnCompositeProject fromProject ) {
            ProjectId = fromProject.EntityId;
            Name = fromProject.Name;
            Description = fromProject.Description;
            IssuePrefix = fromProject.Project.IssuePrefix;
            NextIssueNumber = fromProject.Project.NextIssueNumber;
        }
    }

    public class EditProjectResponse : BaseResponse {
        public  SnCompositeProject ?    Project { get; }

        [JsonConstructor]
        public EditProjectResponse( bool succeeded, string message, SnCompositeProject project ) :
            base( succeeded, message ) {
            Project = project;
        }

        public EditProjectResponse( SnCompositeProject project ) {
            Project = project;
        }

        public EditProjectResponse( Exception ex ) :
            base( ex ) {
            Project = null;
        }

        public EditProjectResponse( string message ) :
            base( false, message ) {
            Project = null;
        }

        public EditProjectResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Project = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class EditProjectRequestValidator : AbstractValidator<EditProjectRequest> {
        public EditProjectRequestValidator() {
            RuleFor( p => p.Name )
                .NotEmpty()
                .WithMessage( "Projects must have a name" );

            RuleFor( p => p.Name )
                .MaximumLength( 100 )
                .WithMessage( "Project names must be less than 100 characters" );

            RuleFor( p => p.Description )
                .MaximumLength( 150 )
                .WithMessage( "Project descriptions must be less than 150 characters" );

            RuleFor( p => p.IssuePrefix )
                .NotEmpty()
                .WithMessage( "An Issue prefix must be specified" );

            RuleFor( p => p.IssuePrefix )
                .MaximumLength( 11 )
                .WithMessage( "Issue prefixes must be 10 characters or less" );

            RuleFor( p => p.NextIssueNumber )
                .GreaterThan( (uint)0 )
                .WithMessage( "The next issue number must be greater than zero" );

            RuleFor( p => p.NextIssueNumber )
                .LessThanOrEqualTo( (uint)10001 )
                .WithMessage( "The next issue number must be less than 10,000" );
        }
    }
}
