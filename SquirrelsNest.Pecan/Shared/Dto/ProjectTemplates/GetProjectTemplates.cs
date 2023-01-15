using System.Collections.Generic;
using SquirrelsNest.Pecan.Shared.Constants;
using System.Text.Json.Serialization;
using SquirrelsNest.Pecan.Shared.Entities;
using System;
using FluentValidation;
using FluentValidation.Results;

namespace SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates {
    public class GetProjectTemplatesRequest {
        public  PageRequest PageRequest { get; }

        public const string Route = $"{Routes.BaseRoute}/getProjectTemplates";

        [JsonConstructor]
        public GetProjectTemplatesRequest( PageRequest pageRequest ) {
            PageRequest = pageRequest;
        }
    }

    public class GetProjectTemplatesResponse : BaseResponse {
        public  PageInformation             PageInformation { get; }
        public  List<SnProjectTemplate>     Templates { get; }

        [JsonConstructor]
        public GetProjectTemplatesResponse( List<SnProjectTemplate> templates, PageInformation pageInformation ) {
            PageInformation = pageInformation;
            Templates = templates;
        }

        public GetProjectTemplatesResponse() {
            PageInformation = PageInformation.Default;
            Templates = new List<SnProjectTemplate>();
        }

        public GetProjectTemplatesResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            PageInformation = PageInformation.Default;
            Templates = new List<SnProjectTemplate>();
        }

        public GetProjectTemplatesResponse( Exception ex ) :
            base( ex ) {
            PageInformation = PageInformation.Default;
            Templates = new List<SnProjectTemplate>();
        }
    }

    // ReSharper disable once UnusedType.Global
    public class GetProjectTemplatesRequestValidator : AbstractValidator<GetProjectTemplatesRequest> {
        public GetProjectTemplatesRequestValidator() {
            RuleFor( p => p.PageRequest )
                .SetValidator( new PageRequestValidator());
        }
    }
}
