using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;
using FluentValidation;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Shared.Dto.Issues {
    public class GetIssuesRequest {
        public  string  ProjectId { get; }

        public const string Route = $"{Routes.BaseRoute}/getIssues";

        [JsonConstructor]
        public GetIssuesRequest( string projectId ) {
            ProjectId = projectId;
        }
    }

    public class GetIssuesResponse : BaseResponse {
        public List<SnCompositeIssue>   Issues { get; }

        [JsonConstructor]
        public GetIssuesResponse( bool succeeded, string message, List<SnCompositeIssue> issues ) :
            base( succeeded, message ) {
            Issues = issues;
        }

        public GetIssuesResponse() {
            Issues = new List<SnCompositeIssue>();
        }

        public GetIssuesResponse( List<SnCompositeIssue> issues ) {
            Issues = new List<SnCompositeIssue>( issues );
        }

        public GetIssuesResponse( string message ) :
            base( false, message ) {
            Issues = new List<SnCompositeIssue>();
        }

        public GetIssuesResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Issues = new List<SnCompositeIssue>();
        }

        public GetIssuesResponse( Exception ex ) :
            base( ex ) {
            Issues = new List<SnCompositeIssue>();
        }
    }

    // ReSharper disable once UnusedType.Global
    public class GetIssuesRequestValidator : AbstractValidator<GetIssuesRequest> {
        public GetIssuesRequestValidator() {
            RuleFor( p => p.ProjectId ).NotEmpty().WithMessage( "Project identifier must be specified" );
        }
    }
}
