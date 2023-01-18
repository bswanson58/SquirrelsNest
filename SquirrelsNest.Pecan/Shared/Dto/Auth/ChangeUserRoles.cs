﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.Auth {
    public class ChangeUserRolesRequest {
        public  SnUser          User { get; }
        public  List<string>    Roles { get; }
        public  bool            DisableRoles { get; set; }

        public const string Route = $"{Routes.BaseRoute}/changeUserRoles";

        [JsonConstructor]
        public ChangeUserRolesRequest( SnUser user, List<string> roles, bool disableRoles ) {
            User = user;
            Roles = roles;
            DisableRoles = disableRoles;
        }

        public ChangeUserRolesRequest( SnUser user ) {
            User = user;
            Roles = new List<string>();
            DisableRoles = false;
        }
    }

    public class ChangeUserRolesResponse : BaseResponse {
        public  SnUser ?    User {  get; }

        [JsonConstructor]
        public ChangeUserRolesResponse( bool succeeded, string message, SnUser user ) :
            base( succeeded, message ) {
            User = user;
        }

        public ChangeUserRolesResponse( SnUser user ) {
            User = user;
        }

        public ChangeUserRolesResponse( Exception ex ) :
            base( ex ) {
            User = null;
        }

        public ChangeUserRolesResponse( string message ) :
            base( false, message ) {
            User = null;
        }

        public ChangeUserRolesResponse( ValidationResult validationResult ) :
            base( validationResult ) {
            User = null;
        }

        public ChangeUserRolesResponse( IEnumerable<string> errors ) :
            base( false, string.Join( Environment.NewLine, errors )) {
            User = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class ChangeUserRolesRequestValidator : AbstractValidator<ChangeUserRolesRequest> {
        public ChangeUserRolesRequestValidator() {
            RuleFor( p => p.User )
                .NotNull()
                .WithMessage( "User must be specified" );

            RuleFor( p => p.User.EntityId )
                .NotNull()
                .WithMessage( "User is invalid" );

            RuleFor( p => p.User.EntityId )
                .MaximumLength( 50 )
                .WithMessage( "User is invalid" );

            RuleFor( p => p.Roles )
                .NotNull()
                .WithMessage( "Roles must be specified" );

            RuleFor( p => p.Roles )
                .Must( r => r.Count < 5 )
                .WithMessage( "Rules are invalid" );

            RuleFor( p => p.Roles )
                .ForEach( r => r.NotEmpty())
                .ForEach( r => r.MaximumLength( 32 ))
                .WithMessage( "Rules are invalid" );
        }
    }
}
