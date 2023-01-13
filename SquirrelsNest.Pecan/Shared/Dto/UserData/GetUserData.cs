using System;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Shared.Dto.UserData {
    public class GetUserDataRequest {
        public  string  DataType { get; }

        public const string Route = $"{Routes.BaseRoute}/getUserData";

        [JsonConstructor]
        public GetUserDataRequest( string dataType ) {
            DataType = dataType;
        }
    }

    public class GetUserDataResponse : BaseResponse {
        public  string  DataType { get; }
        public  string  UserData {  get; }

        [JsonConstructor]
        public GetUserDataResponse( bool succeeded, string message, string dataType, string userData ) :
            base( succeeded, message ) {
            DataType = dataType;
            UserData = userData;
        }

        public GetUserDataResponse( string dataType, string userData ) {
            DataType = dataType;
            UserData = userData;
        }

        public GetUserDataResponse( string message ) :
            base( false, message ) {
            DataType = String.Empty;
            UserData = String.Empty;
        }

        public GetUserDataResponse( Exception ex ) :
            base( ex ) {
            DataType = String.Empty;
            UserData = String.Empty;
        }

        public GetUserDataResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            DataType = String.Empty;
            UserData = String.Empty;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class ValidateGetUserDataRequest : AbstractValidator<GetUserDataRequest> {
        public ValidateGetUserDataRequest() {
            RuleFor( p => p.DataType )
                .NotEmpty()
                .WithMessage( "User DataType is required" );

            RuleFor( p => p.DataType )
                .MaximumLength( 20 )
                .WithMessage( "DataType specifier to too long" );
        }
    }
}
