using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Transfer.Export;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ExportProjectDialogViewModel : DialogAwareBase {
        public const string                 cExportParameters = "parameters";
        public const string                 cUserParameter = "user";

        private readonly IProjectProvider   mProjectProvider;
        private ExportParameters ?          mParameters;
        private SnProject ?                 mCurrentProject;
        private Option<SnUser>              mCurrentUser;
        private string                      mExportPath;
        private bool                        mIncludeCompletedIssues;

        public  RangeCollection<SnProject>  ProjectList { get; }

        public ExportProjectDialogViewModel( IProjectProvider projectProvider ) {
            mProjectProvider = projectProvider;

            ProjectList = new RangeCollection<SnProject>();
            mIncludeCompletedIssues = false;
            mCurrentProject = SnProject.Default;
            mCurrentUser = Option<SnUser>.None;
            mExportPath = String.Empty;

            SetTitle( "Export Project Properties" );

            if( mParameters != null ) {
                IncludeCompletedProjects = mParameters.IncludeCompletedIssues;
                ExportPath = mParameters.ExportFilePath;
                CurrentProject = ProjectList.FirstOrDefault( p => p.EntityId.Equals( mParameters.Project.EntityId ));
            }
        }

        public override async void OnDialogOpened( IDialogParameters parameters ) {
            mParameters = parameters.GetValue<ExportParameters>( cExportParameters );
            mCurrentUser = parameters.GetValue<Option<SnUser>>( cUserParameter );

            await LoadProjects();
        }

        public bool IncludeCompletedProjects {
            get => mIncludeCompletedIssues;
            set => SetProperty( ref mIncludeCompletedIssues, value );
        }

        public SnProject ? CurrentProject {
            get => mCurrentProject;
            set => SetProperty( ref mCurrentProject, value );
        }

        public string ExportPath {
            get => mExportPath;
            set => SetProperty( ref mExportPath, value );
        }

        private async Task LoadProjects() {
            if( mCurrentUser.IsSome ) {
                var projects = await mCurrentUser.MapAsync( user => mProjectProvider.GetProjects( user ));

                projects.Do( list => ProjectList.Reset( list ));
            }
        }

        protected override void OnAccept() {
            if( mCurrentProject != null ) {
                var exportParameters = new ExportParameters( mCurrentProject, IncludeCompletedProjects, ExportPath );
                var parameters = new DialogParameters{{ cExportParameters, exportParameters }};

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, parameters ));
            }
        }
    }
}
