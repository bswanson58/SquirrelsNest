using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IssueTypeViewModel : ObservableObject {
        private readonly IIssueTypeProvider mIssueTypeProvider;
        private readonly IDialogService     mDialogService;
        private readonly IDisposable        mStateSubscription;
        private readonly ILog               mLog;
        private SnProject ?                 mCurrentProject;

        public  ObservableCollection<SnIssueType> IssueTypeList { get; }

        public  IRelayCommand                   CreateIssueType { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public  IRelayCommand<SnIssueType>      EditIssueType { get; }

        public IssueTypeViewModel( IIssueTypeProvider issueTypeProvider, IModelState modelState, IDialogService dialogService, ILog log ) {
            mIssueTypeProvider = issueTypeProvider;
            mDialogService = dialogService;
            mLog = log;

            IssueTypeList = new ObservableCollection<SnIssueType>();
            CreateIssueType = new RelayCommand( OnCreateIssueType );
            EditIssueType = new RelayCommand<SnIssueType>( OnEditIssueType );

            mStateSubscription = modelState.OnStateChange.Subscribe( OnStateChanged );
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project.Do( project => {
                LoadIssueTypes( project );

                mCurrentProject = project;
            });
        }

        private void LoadIssueTypes( SnProject forProject ) {
            IssueTypeList.Clear();

            mIssueTypeProvider
                .GetIssues( forProject ).Result
                .Match( list => list.ForEach( p => IssueTypeList.Add( p )),
                    error => mLog.LogError( error ));
        }

        private void OnCreateIssueType() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters();

                mDialogService.ShowDialog( nameof( EditIssueTypeDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var issueType = result.Parameters.GetValue<SnIssueType>( EditIssueTypeDialogViewModel.cIssueTypeParameter );

                        if( issueType == null ) throw new ApplicationException( "SnIssueType was not returned when editing issue" );

                        mIssueTypeProvider
                            .AddIssue( issueType.For( mCurrentProject )).Result
                            .Match( _ => LoadIssueTypes( mCurrentProject ),
                                error => mLog.LogError( error ));
                    }
                });
            }
        }

        private void OnEditIssueType( SnIssueType ? editIssueType ) {
            if(( mCurrentProject != null ) &&
               ( editIssueType != null )) {
                var parameters = new DialogParameters{{ EditIssueTypeDialogViewModel.cIssueTypeParameter, editIssueType }};

                mDialogService.ShowDialog( nameof( EditIssueTypeDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var issueType = result.Parameters.GetValue<SnIssueType>( EditIssueTypeDialogViewModel.cIssueTypeParameter );

                        if( issueType == null ) throw new ApplicationException( "SnIssueType was not returned when editing issue" );

                        mIssueTypeProvider
                            .UpdateIssue( issueType.For( mCurrentProject )).Result
                            .Match( _ => LoadIssueTypes( mCurrentProject ),
                                error => mLog.LogError( error ));
                    }
                });
            }
        }

        public void Dispose() {
            mIssueTypeProvider.Dispose();
            mStateSubscription.Dispose();
        }
    }
}
