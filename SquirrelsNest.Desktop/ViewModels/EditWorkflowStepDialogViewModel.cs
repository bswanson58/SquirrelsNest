using System;
using System.ComponentModel.DataAnnotations;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    internal class EditWorkflowStepDialogViewModel : DialogAwareBase {
        public  const string            cStateParameter = "state";

        private SnWorkflowState ?       mWorkflowState;
        private string                  mWorkflowName;
        private string                  mWorkflowDescription;

        public  bool                    IsInitialState { get; set; }
        public  bool                    IsFinalState { get; set; }
        public  bool                    IsTerminalState { get; set; }

        public EditWorkflowStepDialogViewModel() {
            SetTitle( "Workflow State Properties" );

            mWorkflowName = String.Empty;
            mWorkflowDescription = String.Empty;
            IsInitialState = false;
            IsTerminalState = false;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mWorkflowState = parameters.GetValue<SnWorkflowState>( cStateParameter );

            if( mWorkflowState != null ) {
                mWorkflowDescription = mWorkflowState.Description;
                mWorkflowName = mWorkflowState.Name;
                IsInitialState = mWorkflowState.IsInitialState;
                IsFinalState = mWorkflowState.IsFinalState;
                IsTerminalState = mWorkflowState.IsTerminalState;
            }
        }

        [Required( ErrorMessage = "Name is required" )]
        [MinLength( 3, ErrorMessage = "Workflow names must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Workflow names must be less than 100 characters" )]
        public string Name {
            get => mWorkflowName;
            set => SetProperty( ref mWorkflowName, value, true );
        }

        public string Description {
            get => mWorkflowDescription;
            set => SetProperty( ref mWorkflowDescription, value, true );
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if(!HasErrors ) {
                var state = mWorkflowState ?? new SnWorkflowState( Name );

                state = state.With( name: Name, description: Description, isInitialState: IsInitialState, isFinalState: IsFinalState, isTerminalState: IsTerminalState );

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters {{ cStateParameter, state }}));
            }
        }
    }
}
