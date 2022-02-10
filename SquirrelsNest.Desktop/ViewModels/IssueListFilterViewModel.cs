using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MoreLinq;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;

namespace SquirrelsNest.Desktop.ViewModels {
    internal class IssueListFilterViewModel : ObservableObject {
        private readonly IProjectProvider       mProjects;
        private SnProject ?                     mCurrentProject;

        public  ObservableCollection<SnProject> ProjectList { get; }
        public  IRelayCommand                   CreateProject { get; }

        public IssueListFilterViewModel( IProjectProvider projects ) {
            mProjects = projects;

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
                .IfLeft( error => { });

            if( currentProject != null ) {
                CurrentProject = ProjectList.FirstOrDefault( p => p.EntityId.Equals( currentProject.EntityId ));
            }

            CurrentProject ??= ProjectList.FirstOrDefault();
        }

        private void OnCreateProject() { }
    }
}
