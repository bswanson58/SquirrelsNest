using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditProjectDialogViewModel : ObservableValidator, IDialogAware {
        public  const string    cProject = "project";

        private SnProject ?     mProject;
        private string          mName;
        private string          mIssuePrefix;
        private string          mDescription;

        public  IRelayCommand   Ok { get; }
        public  IRelayCommand   Cancel { get; }

        public string           Title { get; }

        public event Action<IDialogResult> ? RequestClose;

        public EditProjectDialogViewModel() {
            Name = String.Empty;
            Title = "Project Properties";

            mName = String.Empty;
            mIssuePrefix = String.Empty;
            mDescription = String.Empty;

            Ok = new RelayCommand( OnOk, CanExecuteOk );
            Cancel = new RelayCommand( OnCancel );

            ErrorsChanged += OnErrorsUpdated;
        }

        public void OnDialogOpened( IDialogParameters parameters ) {
            mProject = parameters.GetValue<SnProject>( cProject );

            if( mProject != null ) {
                Name = mProject.Name;
                IssuePrefix = mProject.IssuePrefix;
                Description = mProject.Description;

                OnPropertyChanged( nameof( Name ));
                OnPropertyChanged( nameof( IssuePrefix ));
                OnPropertyChanged( nameof( Description ));
            }

            ValidateAllProperties();
        }

        [Required( ErrorMessage = "Name is required" )]
        [MinLength( 3, ErrorMessage = "Project names must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Project names must be less than 100 characters" )]
        public string Name {
            get => mName;
            set => SetProperty( ref mName, value, true );
        }

        [Required]
        [MinLength(1)]
        [MaxLength(8)]
        public string IssuePrefix {
            get => mIssuePrefix;
            set => SetProperty( ref mIssuePrefix, value, true );
        }

        public string Description {
            get => mDescription;
            set => SetProperty( ref mDescription, value, true );
        }

        private void OnOk() {
            if(!HasErrors ) {
                var project = mProject ?? new SnProject( Name, IssuePrefix );

                project = project.With( name: Name, description: Description, issuePrefix: IssuePrefix );

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters {{ cProject, project }}));
            }
        }

        private bool CanExecuteOk() => !HasErrors;

        private void OnCancel() {
            RaiseRequestClose(new DialogResult( ButtonResult.Cancel, new DialogParameters()));
        }

        protected virtual void RaiseRequestClose( IDialogResult dialogResult ) {
            RequestClose?.Invoke( dialogResult );
        }

        private void OnErrorsUpdated( object ? sender, DataErrorsChangedEventArgs args ) {
            Ok.NotifyCanExecuteChanged();
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
    }
}
