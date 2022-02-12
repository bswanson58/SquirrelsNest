using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    internal class IssueListFilterViewModel : ObservableObject {
        private readonly IProjectProvider       mProjects;
        private readonly IDialogService         mDialogService;
        private SnProject ?                     mCurrentProject;

        public  ObservableCollection<SnProject> ProjectList { get; }
        public  IRelayCommand                   CreateProject { get; }

        public IssueListFilterViewModel( IProjectProvider projects, IDialogService dialogService ) {
            mProjects = projects;
            mDialogService = dialogService;

            ProjectList = new ObservableCollection<SnProject>();

            CreateProject = new RelayCommand( OnCreateProject );

            LoadProjects();
        }

        public SnProject ?  CurrentProject {
            get => mCurrentProject;
            set => SetProperty( ref mCurrentProject, value );
        }

        private void LoadProjects() {
            var currentProject = CurrentProject;

            ProjectList.Clear();

            mProjects.GetProjects()
                .Do( projectList => projectList.ForEach( project => ProjectList.Add( project )))
                .IfLeft( _ => { });

            if( currentProject != null ) {
                CurrentProject = ProjectList.FirstOrDefault( p => p.EntityId.Equals( currentProject.EntityId ));
            }

            CurrentProject ??= ProjectList.FirstOrDefault();
        }

        private void OnCreateProject() {
            var parameters = new DialogParameters();

            mDialogService.ShowDialog( nameof( EditProjectDialog ), parameters, result => {
                if( result.Result == ButtonResult.Ok ) {
                    var editedProject = result.Parameters.GetValue<SnProject>( EditProjectDialogViewModel.cProject );

                    if( editedProject != null ) {
                        mProjects.AddProject( editedProject )
                            .IfLeft( _ => { });
                    }
                }
            });
        }
    }
}
