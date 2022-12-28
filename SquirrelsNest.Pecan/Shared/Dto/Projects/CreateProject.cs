using FluentValidation;
using System;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Entities;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class CreateProjectInput {
        public  string      Name { get; set; }
        public  string      Description { get; set; }
        public  string      IssuePrefix { get; set; }

        public const string Route = $"{Routes.BaseRoute}/createProject";

        public CreateProjectInput() {
            Name = String.Empty;
            Description = String.Empty;
            IssuePrefix = String.Empty;
        }
    }

    public class CreateProjectResponse : BaseResponse {
        public  SnCompositeProject ?    Project { get; }

        [JsonConstructor]
        public CreateProjectResponse( bool succeeded, string message, SnCompositeProject project ) :
            base( succeeded, message ) {
            Project = project;
        }

        public CreateProjectResponse( SnCompositeProject project ) {
            Project = project;
        }

        public CreateProjectResponse( Exception ex ) :
            base( ex ) {
            Project = null;
        }

        public CreateProjectResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Project = null;
        }
    }

    public class CreateProjectInputValidator : AbstractValidator<CreateProjectInput> {
        public CreateProjectInputValidator() {
            RuleFor( p => p.Name )
                .NotEmpty()
                .WithMessage( "A name for the project is required." );

            RuleFor( p => p.Description )
                .NotNull()
                .WithMessage( "The project description may not be null." );

            RuleFor( p => p.IssuePrefix )
                .NotEmpty()
                .WithMessage( "An issue prefix is required." );
        }
    }
}
