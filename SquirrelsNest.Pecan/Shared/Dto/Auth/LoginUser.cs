﻿using FluentValidation;
using SquirrelsNest.Pecan.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Shared.Dto.Auth {
    public class LoginUserInput {
        public  string  LoginName { get; set; }
        public  string  Password {  get; set; }

        public const string Route = $"{Routes.BaseRoute}/loginUser";

        public LoginUserInput() {
            LoginName = String.Empty;
            Password = String.Empty;
        }
    }

    public class LoginUserResponse : BaseResponse {
        public  string      Token { get; set; }
        public  DateTime    Expiration { get; set; }

        [JsonConstructor]
        public LoginUserResponse( bool succeeded, string message, string token, DateTime expiration ) :
            base( succeeded, message ) {
            Token = token;
            Expiration = expiration;
        }

        public LoginUserResponse( string token, DateTime expiration ) {
            Token = token;
            Expiration = expiration;
        }

        public LoginUserResponse( Exception ex ) :
            base( ex ) {
            Token = String.Empty;
            Expiration = DateTimeProvider.Instance.CurrentDateTime;
        }

        public LoginUserResponse( string error ) :
            base( false, error  ) {
            Token = String.Empty;
            Expiration = DateTimeProvider.Instance.CurrentDateTime;
        }

        public LoginUserResponse( IEnumerable<string> errors ) :
            base( false, string.Join( Environment.NewLine, errors ) ) {
            Token = String.Empty;
            Expiration = DateTimeProvider.Instance.CurrentDateTime;
        }
    }

    public class LoginUserInputValidator : AbstractValidator<LoginUserInput> {
        public LoginUserInputValidator() {
            RuleFor( p => p.LoginName ).NotEmpty().WithMessage( "User name is required." );
            RuleFor( p => p.Password ).MinimumLength( 6 ).WithMessage( "Password must be at least 6 characters" );
        }
    }
}