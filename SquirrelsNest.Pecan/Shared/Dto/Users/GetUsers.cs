using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.Users {
    public class GetUsersRequest {
        public  PageRequest PageRequest { get; }

        public const string Route = $"{Routes.BaseRoute}/getUsers";

        [JsonConstructor]
        public GetUsersRequest( PageRequest pageRequest ) {
            PageRequest = pageRequest;
        }
    }

    public class GetUsersResponse : BaseResponse {
        public  PageInformation     PageInformation { get; }
        public  List<SnUser>        Users { get; }

        [JsonConstructor]
        public GetUsersResponse( bool succeeded, string message, List<SnUser> users, PageInformation pageInformation ) :
            base( succeeded, message ) {
            PageInformation = pageInformation;
            Users = users;
        }

        public GetUsersResponse() {
            Users = new List<SnUser>();
            PageInformation = PageInformation.Default;
        }

        public GetUsersResponse( List<SnUser> users, PageInformation pageInformation ) {
            Users = new List<SnUser>( users );
            PageInformation = pageInformation;
        }

        public GetUsersResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Users = new List<SnUser>();
            PageInformation = PageInformation.Default;
        }

        public GetUsersResponse( Exception ex ) :
            base( ex ) {
            Users = new List<SnUser>();
            PageInformation = PageInformation.Default;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest> {
        public GetUsersRequestValidator() {
            RuleFor( p => p.PageRequest )
                .SetValidator( new PageRequestValidator());
        }
    }
}
