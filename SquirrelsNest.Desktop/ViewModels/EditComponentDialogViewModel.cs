using System;
using System.ComponentModel.DataAnnotations;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditComponentDialogViewModel : DialogAwareBase {
        public  const string    cComponentParameter = "component";

        private SnComponent ?   mComponent;
        private string          mComponentName;
        private string          mComponentDescription;

        public EditComponentDialogViewModel() {
            SetTitle( "Component Properties" );

            mComponentName = String.Empty;
            mComponentDescription = String.Empty;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mComponent = parameters.GetValue<SnComponent>( cComponentParameter );

            if( mComponent != null ) {
                mComponentDescription = mComponent.Description;
                mComponentName = mComponent.Name;

                OnPropertyChanged( nameof( Description ));
                OnPropertyChanged( nameof( Name ));
            }
        }

        [Required( ErrorMessage = "Name is required" )]
        [MinLength( 3, ErrorMessage = "Component names must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Component names must be less than 100 characters" )]
        public string Name {
            get => mComponentName;
            set => SetProperty( ref mComponentName, value, true );
        }

        public string Description {
            get => mComponentDescription;
            set => SetProperty( ref mComponentDescription, value, true );
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if(!HasErrors ) {
                var release = mComponent ?? new SnComponent( Name );

                release = release.With( name: Name, description: Description );

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters {{ cComponentParameter, release }}));
            }
        }
    }
}
