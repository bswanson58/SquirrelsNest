using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Transfer.Import;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    internal class ImportProjectDialogViewModel : DialogAwareBase {
        public  const string                cImportParameters = "parameters";
        public  const string                cUserParameter = "user";

        private readonly IProjectProvider   mProjectProvider;
        private readonly List<SnProject>    mExistingProjects;
        private Option<SnUser>              mCurrentUser;
        private string                      mImportPath;
        private string                      mProjectName;

        public ImportProjectDialogViewModel( IProjectProvider projectProvider ) {
            mProjectProvider = projectProvider;

            mExistingProjects = new List<SnProject>();
            mImportPath = String.Empty;
            mProjectName = String.Empty;
            mCurrentUser = Option<SnUser>.None;

            SetTitle( "Import Project" );
        }

        public override async void OnDialogOpened( IDialogParameters parameters ) {
            var inputParameters = parameters.GetValue<ImportParameters>( cImportParameters );
            mCurrentUser = parameters.GetValue<Option<SnUser>>( cUserParameter );

            if( inputParameters != null ) {
                ImportPath = inputParameters.ImportFilePath;
                ProjectName = inputParameters.ProjectName;
            }

            await LoadExistingProjects();
        }

        public string ImportPath {
            get => mImportPath;
            set => SetProperty( ref mImportPath, value );
        }

        public string ProjectName {
            get => mProjectName;
            set => SetProperty( ref mProjectName, value );
        }

        private async Task LoadExistingProjects() {
            if( mCurrentUser.IsSome ) {
                var projects = await mCurrentUser.MapAsync( user => mProjectProvider.GetProjects( user ));

                mExistingProjects.Clear();
                projects.Do( list => mExistingProjects.AddRange( list ));
            }
        }

        protected override void OnAccept() {
            var importParameters = new ImportParameters( ImportPath, ProjectName );
            var parameters = new DialogParameters{{ cImportParameters, importParameters }};

            RaiseRequestClose( new DialogResult( ButtonResult.Ok, parameters ));
        }
    }
}
