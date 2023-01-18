using System;
using FluentValidation;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class ImportProjectRequest {
        public  string      Name { get; set; }
        public  string      Description { get; set; }
        public  string      IssuePrefix { get; set; }
        public  string      ImportContent { get; set; }

        public const string Route = $"{Routes.BaseRoute}/importProject";

        [JsonConstructor]
        public ImportProjectRequest() {
            Name = String.Empty;
            Description = String.Empty;
            IssuePrefix = String.Empty;
            ImportContent = "{}";
        }
    }

    public class ImportProjectResponse : BaseResponse {
        public  SnCompositeProject ?    Project { get; }

        [JsonConstructor]
        public ImportProjectResponse( bool succeeded, string message, SnCompositeProject project ) :
            base( succeeded, message ) {
            Project = project;
        }

        public ImportProjectResponse( SnCompositeProject project ) {
            Project = project;
        }

        public ImportProjectResponse( Exception ex ) :
            base( ex ) {
            Project = null;
        }

        public ImportProjectResponse( string message ) :
            base( false, message ) {
            Project = null;
        }

        public ImportProjectResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Project = null;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ImportProjectRequestValidator : AbstractValidator<ImportProjectRequest> {
        public ImportProjectRequestValidator() {
            RuleFor( p => p.Name )
                .NotEmpty()
                .WithMessage( "A name for the project is required." );

            RuleFor( p => p.Description )
                .NotNull()
                .WithMessage( "The project description may not be null." );

            RuleFor( p => p.IssuePrefix )
                .NotEmpty()
                .WithMessage( "An issue prefix is required." );

            RuleFor( p => p.ImportContent )
                .NotEmpty()
                .WithMessage( "Import content must contain project data" );
        }
    }
}
