using System;
using System.Linq;
using System.Threading.Tasks;
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

        private readonly IProjectProvider   mProjectProvider;
        private ExportParameters ?          mParameters;
        private SnProject ?                 mCurrentProject;
        private string                      mExportPath;
        private bool                        mIncludeCompletedIssues;

        public  RangeCollection<SnProject>  ProjectList { get; }

        public ExportProjectDialogViewModel( IProjectProvider projectProvider ) {
            mProjectProvider = projectProvider;

            ProjectList = new RangeCollection<SnProject>();
            mIncludeCompletedIssues = false;
            mCurrentProject = SnProject.Default;
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
            var projects = await mProjectProvider.GetProjects();

            projects.Do( list => ProjectList.Reset( list ));
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
