using FluentValidation;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;
using System;
using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class IssueTypeChangeInput {
        public  EntityChangeType    ChangeType { get; }
        public  SnIssueType         IssueType { get; }

        public  const string        Route = $"{Routes.BaseRoute}/changeIssueType";

        [JsonConstructor]
        public IssueTypeChangeInput( SnIssueType issueType, EntityChangeType changeType ) {
            ChangeType = changeType;
            IssueType = issueType;
        }
    }

    public class IssueTypeChangeResponse : BaseResponse {
        public  EntityChangeType    ChangeType { get; set; }
        public  SnIssueType ?       IssueType { get; }

        [JsonConstructor]
        public IssueTypeChangeResponse( bool succeeded, string message, EntityChangeType changeType, SnIssueType issueType ) :
            base( succeeded, message ) {
            ChangeType = changeType;
            IssueType = issueType;
        }

        public IssueTypeChangeResponse( SnIssueType issueType, EntityChangeType changeType ) {
            ChangeType = changeType;
            IssueType = issueType;
        }

        public IssueTypeChangeResponse( Exception ex ) :
            base( ex ) {
            IssueType = null;
        }

        public IssueTypeChangeResponse( string message ) :
            base( false, message ) {
            IssueType = null;
        }

        public IssueTypeChangeResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            IssueType = null;
        }

    }

    // ReSharper disable once UnusedType.Global
    public class IssueTypeChangeValidator : AbstractValidator<IssueTypeChangeInput> {
        public IssueTypeChangeValidator() {
            RuleFor( p => p.IssueType ).NotNull().WithMessage( "IssueType to change was null" );
            RuleFor( p => p.IssueType.EntityId ).NotEmpty().WithMessage( "EntityId must not be empty" );
            RuleFor( p => p.IssueType.EntityId ).NotEqual( EntityIdentifier.Default ).WithMessage( "EntityId must not be 'default'" );
            RuleFor( p => p.IssueType.ProjectId ).NotEmpty().WithMessage( "ProjectId must not be empty" );
            RuleFor( p => p.IssueType.ProjectId ).NotEqual( EntityIdentifier.Default ).WithMessage( "ProjectId must not be 'default'" );
            RuleFor( p => p.ChangeType ).IsInEnum().WithMessage( "Change value is not valid" );
            RuleFor( p => p.IssueType.Name ).NotEmpty().WithMessage( "IssueType name must not be empty" );
            RuleFor( p => p.IssueType.ProjectId ).NotEmpty().WithMessage( "ProjectId must not be empty" );
        }
    }
}
