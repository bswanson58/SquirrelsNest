using SquirrelsNest.Pecan.Shared.Constants;
using System.Text.Json.Serialization;
using SquirrelsNest.Pecan.Shared.Entities;
using System;
using FluentValidation;
using FluentValidation.Results;

namespace SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates {
    public class CreateProjectFromTemplateRequest {
        public  string  TemplateName { get; }
        public  string  ProjectName { get; }
        public  string  ProjectDescription { get; }
        public  string  IssuePrefix { get; }

        public const string Route = $"{Routes.BaseRoute}/createProjectFromTemplate";

        [JsonConstructor]
        public CreateProjectFromTemplateRequest( string templateName, string projectName, 
                                                 string projectDescription, string issuePrefix ) {
            TemplateName = templateName;
            ProjectName = projectName;
            ProjectDescription = projectDescription;
            IssuePrefix = issuePrefix;
        }
    }

    public class CreateProjectFromTemplateResponse : BaseResponse {
        public  SnCompositeProject ?    Project { get; }

        [JsonConstructor]
        public CreateProjectFromTemplateResponse( bool succeeded, string message, SnCompositeProject project ) :
            base( succeeded, message ) {
            Project = project;
        }

        public CreateProjectFromTemplateResponse( SnCompositeProject project ) {
            Project = project;
        }

        public CreateProjectFromTemplateResponse( Exception ex ) :
            base( ex ) {
            Project = null;
        }

        public CreateProjectFromTemplateResponse( string message ) :
            base( false, message ) {
            Project = null;
        }

        public CreateProjectFromTemplateResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Project = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class CreateProjectFromTemplateRequestValidator : AbstractValidator<CreateProjectFromTemplateRequest> {
        public CreateProjectFromTemplateRequestValidator() {
            RuleFor( p => p.TemplateName )
                .NotEmpty()
                .WithMessage( "Template name must be specified" );

            RuleFor( p => p.TemplateName )
                .MaximumLength( 32 )
                .WithMessage( "Template name is too long" );

            RuleFor( p => p.ProjectName )
                .NotEmpty()
                .WithMessage( "Project name must be specified" );

            RuleFor( p => p.ProjectName )
                .MaximumLength( 32 )
                .WithMessage( "Project name is too long" );

            RuleFor( p => p.ProjectDescription )
                .NotNull();

            RuleFor( p => p.ProjectDescription )
                .MaximumLength( 100 )
                .WithMessage( "Project description is too long" );
        }
    }
}
