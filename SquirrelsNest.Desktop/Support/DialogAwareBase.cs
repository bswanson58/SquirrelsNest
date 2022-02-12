using System;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSupport.DialogService;

namespace SquirrelsNest.Desktop.Support {
    public class DialogAwareBase : ObservableValidator, IDialogAware {
        public  IRelayCommand   AcceptCommand { get; }
        public  IRelayCommand   CancelCommand { get; }

        public  string          Title { get; private set; }

        public event Action<IDialogResult> ? RequestClose;

        protected DialogAwareBase() {
            AcceptCommand = new RelayCommand( OnAccept, CanAccept );
            CancelCommand = new RelayCommand( OnCancel );

            Title = String.Empty;

            ErrorsChanged += OnErrorsUpdated;
        }

        protected virtual void OnErrorsUpdated( object ? sender, DataErrorsChangedEventArgs args ) {
            AcceptCommand.NotifyCanExecuteChanged();
        }

        // IDialogAware:
        protected void SetTitle( string title ) {
            Title = title;
        }

        public virtual void OnDialogOpened( IDialogParameters parameters ) { }

        protected virtual void OnAccept() {
            RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters()));
        }

        protected virtual bool CanAccept() {
            return !HasErrors;
        }

        protected virtual void OnCancel() {
            RaiseRequestClose( new DialogResult( ButtonResult.Cancel, new DialogParameters()));
        }

        public virtual bool CanCloseDialog() {
            return true;
        }

        public virtual void OnDialogClosed() { }

        protected void RaiseRequestClose( IDialogResult dialogResult ) {
            RequestClose?.Invoke( dialogResult );
        }
    }
}
