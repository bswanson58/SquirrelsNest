﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class MainWindowViewModel : ObservableObject {
        private readonly IssuesViewModel            mIssuesViewModel;
        private readonly ProjectManagementViewModel mProjectsViewModel;
        private readonly UserManagementViewModel    mUsersViewModel;
        private ObservableObject                    mContentViewModel;

        public  IRelayCommand       IssuesLayout { get; }
        public  IRelayCommand       ProjectsLayout { get; }
        public  IRelayCommand       UsersLayout { get; }
        public  IRelayCommand       OptionsLayout { get; }

        public MainWindowViewModel( IssuesViewModel issuesVm, ProjectManagementViewModel projectsViewModel, UserManagementViewModel usersViewModel ) {
            mIssuesViewModel = issuesVm;
            mProjectsViewModel = projectsViewModel;
            mUsersViewModel = usersViewModel;

            IssuesLayout = new RelayCommand( OnIssuesLayout );
            ProjectsLayout = new RelayCommand( OnProjectsLayout );
            UsersLayout = new RelayCommand( OnUsersLayout );
            OptionsLayout = new RelayCommand( OnOptionsLayout );

            mContentViewModel = mIssuesViewModel;
        }

        public ObservableObject ContentViewModel {
            get => mContentViewModel;
            set => SetProperty( ref mContentViewModel, value );
        }

        private void OnIssuesLayout() {
            ContentViewModel = mIssuesViewModel;
        }

        private void OnProjectsLayout() {
            ContentViewModel = mProjectsViewModel;
        }

        private void OnUsersLayout() {
            ContentViewModel = mUsersViewModel;
        }

        private void OnOptionsLayout() { }
    }
}
