using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class GetProjectsRequest {
        public  PageRequest PageRequest { get; }

        public const string Route = $"{Routes.BaseRoute}/getProjects";

        [JsonConstructor]
        public GetProjectsRequest( PageRequest pageRequest ) {
            PageRequest = pageRequest;
        }
    }

    public class GetProjectsResponse : BaseResponse {
        public  PageInformation             PageInformation { get; }
        public  List<SnCompositeProject>    Projects { get; }

        [JsonConstructor]
        public GetProjectsResponse( bool succeeded, string message, List<SnCompositeProject> projects, PageInformation pageInformation ) :
            base( succeeded, message ) {
            PageInformation = pageInformation;
            Projects = projects;
        }

        public GetProjectsResponse() {
            Projects = new List<SnCompositeProject>();
            PageInformation = PageInformation.Default;
        }

        public GetProjectsResponse( List<SnCompositeProject> projects, PageInformation pageInformation ) {
            Projects = new List<SnCompositeProject>( projects );
            PageInformation = pageInformation;
        }

        public GetProjectsResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Projects = new List<SnCompositeProject>();
            PageInformation = PageInformation.Default;
        }

        public GetProjectsResponse( Exception ex ) :
            base( ex ) {
            Projects = new List<SnCompositeProject>();
            PageInformation = PageInformation.Default;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class GetProjectsRequestValidator : AbstractValidator<GetProjectsRequest> {
        public GetProjectsRequestValidator() {
            RuleFor( p => p.PageRequest )
                .SetValidator( new PageRequestValidator());
        }
    }
}
