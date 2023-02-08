using System;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Shared.Dto.UserData {
    public class UpdateUserDataRequest {
        public  string      DataType { get; }
        public  string      Data { get; }

        public const string Route = $"{Routes.BaseRoute}/updateUserData";

        [JsonConstructor]
        public UpdateUserDataRequest( string dataType, string data ) {
            Data = data;
            DataType = dataType;
        }
    }

    public class UpdateUserDataResponse : BaseResponse {
        public  SnUserData      UserData { get; }

        [JsonConstructor]
        public UpdateUserDataResponse( bool succeeded, string message, SnUserData userData ) :
            base( succeeded, message ) {
            UserData = userData;
        }

        public UpdateUserDataResponse( SnUserData userData ) {
            UserData = userData;
        }

        public UpdateUserDataResponse( Exception ex ) :
            base( ex ) {
            UserData = SnUserData.Default;
        }

        public UpdateUserDataResponse( string message ) :
            base( false, message ) {
            UserData = SnUserData.Default;
        }

        public UpdateUserDataResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            UserData = SnUserData.Default;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class ValidateUpdateUserDataRequest : AbstractValidator<UpdateUserDataRequest> {
        public ValidateUpdateUserDataRequest() {
            RuleFor( p => p.Data )
                .NotNull()
                .WithMessage( "UserData cannot be null" );

            RuleFor( p => p.Data )
                .MaximumLength( 1024 )
                .WithMessage( "UserData data is too long" );

            RuleFor( p => p.DataType )
                .NotEmpty()
                .WithMessage( "DataType must be specified" );

            RuleFor( p => p.DataType )
                .MaximumLength( 32 )
                .WithMessage( "DatType specifier is too long" );
        }
    }
}