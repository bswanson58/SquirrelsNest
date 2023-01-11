using FluentValidation;
using System;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Entities;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class DeleteProjectRequest {
        public SnCompositeProject   Project { get; }

        public const string Route = $"{Routes.BaseRoute}/deleteProject";

        [JsonConstructor]
        public DeleteProjectRequest( SnCompositeProject project ) {
            Project = project;
        }
    }

    public class DeleteProjectResponse : BaseResponse {
        public  SnCompositeProject ?    Project { get; }

        [JsonConstructor]
        public DeleteProjectResponse( bool succeeded, string message, SnCompositeProject project ) :
            base( succeeded, message ) {
            Project = project;
        }

        public DeleteProjectResponse( SnCompositeProject project ) {
            Project = project;
        }

        public DeleteProjectResponse( Exception ex ) :
            base( ex ) {
            Project = null;
        }

        public DeleteProjectResponse( string message ) :
            base( false, message ) {
            Project = null;
        }

        public DeleteProjectResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Project = null;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class DeleteProjectInputValidator : AbstractValidator<DeleteProjectRequest> {
        public DeleteProjectInputValidator() {
            RuleFor( p => p.Project )
                .NotNull()
                .WithMessage( "A project to be deleted must be specified" );
        }
    }
}
