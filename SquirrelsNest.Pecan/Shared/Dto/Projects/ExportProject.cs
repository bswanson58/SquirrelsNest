using SquirrelsNest.Pecan.Shared.Constants;
using System.Text.Json.Serialization;
using FluentValidation;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class ExportProjectRequest {
        public  string      ProjectId { get; }
        public  bool        IncludeCompletedIssues { get; }

        public  const string    Route = $"{Routes.BaseRoute}/exportProject";

        [JsonConstructor]
        public ExportProjectRequest( string projectId, bool includeCompletedIssues ) {
            ProjectId = projectId;
            IncludeCompletedIssues = includeCompletedIssues;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class ExportProjectRequestValidator : AbstractValidator<ExportProjectRequest> {
        public ExportProjectRequestValidator() {
            RuleFor( p => p.ProjectId )
                .NotEmpty()
                .WithMessage( "Project ID must be specified" );

            RuleFor( p => p.ProjectId )
                .MaximumLength( 48 )
                .WithMessage( "Project ID is incorrect" );
        }
    }
}
