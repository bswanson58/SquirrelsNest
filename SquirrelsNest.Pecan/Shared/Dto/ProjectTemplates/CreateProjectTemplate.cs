using System;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates {
    public class CreateProjectTemplateRequest {
        public  SnCompositeProject  Project { get; }
        public  string              TemplateName { get; set; }
        public  string              TemplateDescription { get; set; }

        public const string Route = $"{Routes.BaseRoute}/createTemplate";

        [JsonConstructor]
        public CreateProjectTemplateRequest( SnCompositeProject project, string templateName, string templateDescription ) {
            Project = project;
            TemplateName = templateName;
            TemplateDescription = templateDescription;
        }

        public CreateProjectTemplateRequest( SnCompositeProject project ) {
            Project = project;
            TemplateName = String.Empty;
            TemplateDescription = String.Empty;
        }
    }

    public class CreateProjectTemplateResponse : BaseResponse {
        public  SnProjectTemplate ? Template { get; }

        [JsonConstructor]
        public CreateProjectTemplateResponse( bool succeeded, string message, SnProjectTemplate template ) :
            base( succeeded, message ) {
            Template = template;
        }

        public CreateProjectTemplateResponse( SnProjectTemplate template ) {
            Template = template;
        }

        public CreateProjectTemplateResponse( Exception ex ) :
            base( ex ) {
            Template = null;
        }

        public CreateProjectTemplateResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Template = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class CreateProjectTemplateRequestValidator : AbstractValidator<CreateProjectTemplateRequest> {
        public CreateProjectTemplateRequestValidator() {
            RuleFor( p => p.Project )
                .NotNull()
                .WithMessage( "Project must be specified" );

            RuleFor( p => p.Project.Name )
                .NotEmpty()
                .WithMessage( "Project must nave a name" );

            RuleFor( p => p.Project.Name )
                .MaximumLength( 32 )
                .WithMessage( "Project name must be 32 characters or less" );

            RuleFor( p => p.Project.Components )
                .Must( c => c.Count < 16 )
                .WithMessage( "Project has too many components" );

            RuleFor( p => p.Project.IssueTypes )
                .Must( i => i.Count < 32 )
                .WithMessage( "Project has too many issue types" );

            RuleFor( p => p.Project.WorkflowStates )
                .Must( s => s.Count < 32 )
                .WithMessage( "Project has too many workflow states" );
        }
    }
}
