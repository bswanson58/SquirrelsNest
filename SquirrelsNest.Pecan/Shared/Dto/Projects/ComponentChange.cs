using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Text.Json.Serialization;
using System;
using FluentValidation;
using FluentValidation.Results;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public enum EntityChangeType {
        Add,
        Delete,
        Update
    }

    public class ComponentChangeInput {
        public  EntityChangeType    ChangeType { get; }
        public  SnComponent         Component { get; }

        public  const string        Route = $"{Routes.BaseRoute}/changeComponent";

        [JsonConstructor]
        public ComponentChangeInput( SnComponent component, EntityChangeType changeType ) {
            ChangeType = changeType;
            Component = component;
        }
    }

    public class ComponentChangeResponse : BaseResponse {
        public  EntityChangeType    ChangeType { get; set; }
        public  SnComponent ?       Component { get; }

        [JsonConstructor]
        public ComponentChangeResponse( bool succeeded, string message, EntityChangeType changeType, SnComponent component ) :
            base( succeeded, message ) {
            ChangeType = changeType;
            Component = component;
        }

        public ComponentChangeResponse( SnComponent component, EntityChangeType changeType ) {
            ChangeType = changeType;
            Component = component;
        }

        public ComponentChangeResponse( Exception ex ) :
            base( ex ) {
            Component = null;
        }

        public ComponentChangeResponse( string message ) :
            base( false, message ) {
            Component = null;
        }

        public ComponentChangeResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            Component = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class ComponentChangeInputValidator : AbstractValidator<ComponentChangeInput> {
        public ComponentChangeInputValidator() {
            RuleFor( p => p.Component ).NotNull().WithMessage( "Component to change was null" );
            RuleFor( p => p.Component.EntityId ).NotEmpty().WithMessage( "EntityId must not be empty" );
            RuleFor( p => p.Component.EntityId ).NotEqual( EntityIdentifier.Default ).WithMessage( "EntityId must not be 'default'" );
            RuleFor( p => p.Component.ProjectId ).NotEmpty().WithMessage( "ProjectId must not be empty" );
            RuleFor( p => p.Component.ProjectId ).NotEqual( EntityIdentifier.Default ).WithMessage( "ProjectId must not be 'default'" );
            RuleFor( p => p.ChangeType ).IsInEnum().WithMessage( "Change value is not valid" );
            RuleFor( p => p.Component.Name ).NotEmpty().WithMessage( "Component name must not be empty" );
            RuleFor( p => p.Component.ProjectId ).NotEmpty().WithMessage( "ProjectId must not be empty" );
        }
    }
}
