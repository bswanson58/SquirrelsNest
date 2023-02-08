using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditWorkflowStepDialogViewModel : DialogAwareBase {
        public  const string            cStateParameter = "state";

        private SnWorkflowState ?       mWorkflowState;
        private string                  mWorkflowName;
        private string                  mWorkflowDescription;
        private StateCategory ?         mCurrentStateCategory;

        public  ObservableCollection<StateCategory> StateCategories { get; }


        public EditWorkflowStepDialogViewModel() {
            SetTitle( "Workflow State Properties" );

            mWorkflowName = String.Empty;
            mWorkflowDescription = String.Empty;
            mCurrentStateCategory = StateCategory.Initial;

            StateCategories = new ObservableCollection<StateCategory>( Enum.GetValues( typeof( StateCategory )).Cast<StateCategory>());
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mWorkflowState = parameters.GetValue<SnWorkflowState>( cStateParameter );

            if( mWorkflowState != null ) {
                Description = mWorkflowState.Description;
                Name = mWorkflowState.Name;
                CurrentStateCategory = mWorkflowState.Category;
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

        public StateCategory ? CurrentStateCategory {
            get => mCurrentStateCategory;
            set => SetProperty( ref mCurrentStateCategory, value, true );
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if(!HasErrors ) {
                var state = mWorkflowState ?? new SnWorkflowState( Name );

                state = state.With( name: Name, description: Description, category: CurrentStateCategory );

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters {{ cStateParameter, state }}));
            }
        }
    }
}
