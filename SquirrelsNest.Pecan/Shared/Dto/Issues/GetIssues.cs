using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;
using FluentValidation;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Shared.Dto.Issues {
    public class GetIssuesRequest {
        public  string      ProjectId { get; }
        public  PageRequest PageRequest { get; }

        public const string Route = $"{Routes.BaseRoute}/getIssues";

        [JsonConstructor]
        public GetIssuesRequest( string projectId, PageRequest pageRequest ) {
            ProjectId = projectId;
            PageRequest = pageRequest;
        }
    }

    public class GetIssuesResponse : BaseResponse {
        public PageInformation          PageInformation { get; }
        public List<SnCompositeIssue>   Issues { get; }

        [JsonConstructor]
        public GetIssuesResponse( bool succeeded, string message, List<SnCompositeIssue> issues, PageInformation pageInformation ) :
            base( succeeded, message ) {
            PageInformation = pageInformation;
            Issues = issues;
        }

        public GetIssuesResponse() {
            Issues = new List<SnCompositeIssue>();
            PageInformation = PageInformation.Default;
        }

        public GetIssuesResponse( List<SnCompositeIssue> issues, PageInformation pageInformation ) {
            Issues = new List<SnCompositeIssue>( issues );
            PageInformation = pageInformation;
        }

        public GetIssuesResponse( string message ) :
            base( false, message ) {
            Issues = new List<SnCompositeIssue>();
            PageInformation = PageInformation.Default;
        }

        public GetIssuesResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Issues = new List<SnCompositeIssue>();
            PageInformation = PageInformation.Default;
        }

        public GetIssuesResponse( Exception ex ) :
            base( ex ) {
            Issues = new List<SnCompositeIssue>();
            PageInformation = PageInformation.Default;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class GetIssuesRequestValidator : AbstractValidator<GetIssuesRequest> {
        public GetIssuesRequestValidator() {
            RuleFor( p => p.ProjectId )
                .NotEmpty()
                .WithMessage( "Project identifier must be specified" );

            RuleFor( p => p.PageRequest )
                .SetValidator( new PageRequestValidator());
        }
    }
}
