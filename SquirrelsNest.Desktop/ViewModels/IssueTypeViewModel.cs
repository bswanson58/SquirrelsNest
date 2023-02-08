using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.Extensions;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Models;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IssueTypeViewModel : ObservableObject, IDisposable {
        private readonly IIssueTypeProvider mIssueTypeProvider;
        private readonly IDialogService     mDialogService;
        private readonly IDisposable        mStateSubscription;
        private readonly ILog               mLog;
        private SnProject ?                 mCurrentProject;

        public  ObservableCollection<SnIssueType> IssueTypeList { get; }

        public  IRelayCommand                   CreateIssueType { get; }
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  IRelayCommand<SnIssueType>      EditIssueType { get; }
        public  IRelayCommand<SnIssueType>      DeleteIssueType { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public IssueTypeViewModel( IIssueTypeProvider issueTypeProvider, IModelState modelState, IDialogService dialogService, ILog log ) {
            mIssueTypeProvider = issueTypeProvider;
            mDialogService = dialogService;
            mLog = log;

            IssueTypeList = new ObservableCollection<SnIssueType>();
            CreateIssueType = new RelayCommand( OnCreateIssueType );
            EditIssueType = new RelayCommand<SnIssueType>( OnEditIssueType );
            DeleteIssueType = new RelayCommand<SnIssueType>( OnDeleteIssueType );

            mStateSubscription = modelState.OnStateChange.SubscribeAsync( OnStateChanged, OnError );
        }

        private async Task<Unit> OnStateChanged( CurrentState state ) {
            state.Project.Do( project => {
                mCurrentProject = project;
            });

            await LoadIssueTypes();

            return Unit.Default;
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During ModelStateChanged/IssueListChanged in {nameof( IssueTypeViewModel )}", ex );
        }

        private async Task LoadIssueTypes() {
            if( mCurrentProject != null ) {
                IssueTypeList.Clear();

                ( await mIssueTypeProvider.GetIssues( mCurrentProject ))
                    .Match( list => list.ForEach( p => IssueTypeList.Add( p )),
                            error => mLog.LogError( error ));
            }
        }

        private void OnCreateIssueType() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters();

                mDialogService.ShowDialog( nameof( EditIssueTypeDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var issueType = result.Parameters.GetValue<SnIssueType>( EditIssueTypeDialogViewModel.cIssueTypeParameter );

                        if( issueType == null ) throw new ApplicationException( "SnIssueType was not returned when editing issue" );

                        ( await mIssueTypeProvider.AddIssue( issueType.For( mCurrentProject )))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadIssueTypes();
                    }
                });
            }
        }

        private void OnEditIssueType( SnIssueType ? editIssueType ) {
            if(( mCurrentProject != null ) &&
               ( editIssueType != null )) {
                var parameters = new DialogParameters{{ EditIssueTypeDialogViewModel.cIssueTypeParameter, editIssueType }};

                mDialogService.ShowDialog( nameof( EditIssueTypeDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var issueType = result.Parameters.GetValue<SnIssueType>( EditIssueTypeDialogViewModel.cIssueTypeParameter );

                        if( issueType == null ) throw new ApplicationException( "SnIssueType was not returned when editing issue" );

                        ( await mIssueTypeProvider.UpdateIssue( issueType.For( mCurrentProject )))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadIssueTypes();
                    }
                });
            }
        }

        private void OnDeleteIssueType( SnIssueType ? issueType ) {
            if( issueType != null ) {
                var parameters = new DialogParameters {
                    { ConfirmationDialogViewModel.cConfirmationText, $"Would you like to delete the issue type named '{issueType.Name}'?" }
                };

                mDialogService.ShowDialog( nameof( ConfirmationDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        ( await mIssueTypeProvider.DeleteIssue( issueType ))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadIssueTypes();
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
