using MvvmSupport.DialogService;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ConfirmationDialogViewModel : DialogAwareBase {
        public  const string        cConfirmationText = "confirmationText";

        public  string ?            ConfirmationText { get; private set; }

        public ConfirmationDialogViewModel() {
            SetTitle( "Confirmation" );
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            ConfirmationText = parameters.GetValue<string>( cConfirmationText );

            OnPropertyChanged( nameof( ConfirmationText ));
        }
    }
}
