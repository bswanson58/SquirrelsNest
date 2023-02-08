using FluentValidation;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Text.Json.Serialization;
using System;
using FluentValidation.Results;

namespace SquirrelsNest.Pecan.Shared.Dto.Projects {
    public class WorkflowStateChangeInput {
        public  EntityChangeType    ChangeType { get; }
        public  SnWorkflowState     WorkflowState { get; }

        public  const string        Route = $"{Routes.BaseRoute}/changeWorkflowState";

        [JsonConstructor]
        public WorkflowStateChangeInput( SnWorkflowState workflowState, EntityChangeType changeType ) {
            ChangeType = changeType;
            WorkflowState = workflowState;
        }
    }

    public class WorkflowStateChangeResponse : BaseResponse {
        public  EntityChangeType    ChangeType { get; }
        public  SnWorkflowState ?   WorkflowState { get; }

        [JsonConstructor]
        public WorkflowStateChangeResponse( bool succeeded, string message, EntityChangeType changeType, SnWorkflowState workflowState ) :
            base( succeeded, message ) {
            ChangeType = changeType;
            WorkflowState = workflowState;
        }

        public WorkflowStateChangeResponse( SnWorkflowState workflowState, EntityChangeType changeType ) {
            ChangeType = changeType;
            WorkflowState = workflowState;
        }

        public WorkflowStateChangeResponse( Exception ex ) :
            base( ex ) {
            WorkflowState = null;
        }

        public WorkflowStateChangeResponse( string message ) :
            base( false, message ) {
            WorkflowState = null;
        }

        public WorkflowStateChangeResponse( ValidationResult validationResult ) :
            base ( validationResult ) {
            WorkflowState = null;
        }
    }

    // ReSharper disable once UnusedType.Global
    public class WorkflowStateChangeInputValidator : AbstractValidator<WorkflowStateChangeInput> {
        public WorkflowStateChangeInputValidator() {
            RuleFor( p => p.WorkflowState ).NotNull().WithMessage( "WorkflowState to change was null" );
            RuleFor( p => p.WorkflowState.EntityId ).NotEmpty().WithMessage( "EntityId must not be empty" );
            RuleFor( p => p.WorkflowState.EntityId ).NotEqual( EntityIdentifier.Default ).WithMessage( "EntityId must not be 'default'" );
            RuleFor( p => p.WorkflowState.ProjectId ).NotEmpty().WithMessage( "ProjectId must not be empty" );
            RuleFor( p => p.WorkflowState.ProjectId ).NotEqual( EntityIdentifier.Default ).WithMessage( "ProjectId must not be 'default'" );
            RuleFor( p => p.ChangeType ).IsInEnum().WithMessage( "Change value is not valid" );
            RuleFor( p => p.WorkflowState.Name ).NotEmpty().WithMessage( "WorkflowState name must not be empty" );
            RuleFor( p => p.WorkflowState.ProjectId ).NotEmpty().WithMessage( "ProjectId must not be empty" );
            RuleFor( p => p.WorkflowState.Category ).IsInEnum().WithMessage( "WorkflowState.Category must be valid enum value" );
        }
    }
}
