using System;
using System.Reactive.Linq;
using System.Threading;
using LanguageExt;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Models;
using SquirrelsNest.Core.ProjectTemplates;
using SquirrelsNest.Core.Transfer.Export;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ProjectPropertiesViewModel : IDisposable {
        private readonly IProjectTemplateManager    mTemplateManager;
        private readonly IExportManager             mExportManager;
        private readonly IDialogService             mDialogService;
        private readonly IDisposable                mStateSubscription;
        private readonly ILog                       mLog;
        private Option<SnUser>                      mCurrentUser;
        private SnProject ?                         mCurrentProject;

        public  IRelayCommand                       CreateTemplate { get; }
        public  IRelayCommand                       ExportProject { get; }

        public ProjectPropertiesViewModel( IModelState modelState, IProjectTemplateManager templateManager, IExportManager exportManager,
                                           IDialogService dialogService, ILog log, SynchronizationContext context ) {
            mLog = log;
            mDialogService = dialogService;
            mTemplateManager = templateManager;
            mExportManager = exportManager;
            mCurrentUser = Option<SnUser>.None;

            CreateTemplate = new RelayCommand( OnCreateTemplate );
            ExportProject = new RelayCommand( OnExportProject );

            mStateSubscription = modelState.OnStateChange.ObserveOn( context ).Subscribe( OnStateChanged );
        }

        private void OnStateChanged( CurrentState state ) {
            mCurrentUser = state.User;

            state.Project.Do( project => {
                mCurrentProject = project;
            });
        }

        private void OnCreateTemplate() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters{{ EditProjectParametersDialogViewModel.cTemplate, new TemplateParameters() }};

                mDialogService.ShowDialog( nameof( EditProjectParametersDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var template = result.Parameters.GetValue<TemplateParameters>( EditProjectParametersDialogViewModel.cTemplate );

                        if( template == null ) throw new ApplicationException( "Dialog did not return template parameters." );

                        ( await mTemplateManager.CreateTemplate( mCurrentProject, template ))
                            .IfLeft( error => mLog.LogError( error ));
                    }
                });
            }
        }

        private void OnExportProject() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters{{ ExportProjectDialogViewModel.cUserParameter, mCurrentUser }};

                mDialogService.ShowDialog( nameof( ExportProjectDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var exportParameters = result.Parameters.GetValue<ExportParameters>( ExportProjectDialogViewModel.cExportParameters );

                        if( exportParameters == null ) throw new ApplicationException( "Dialog did not return export parameters." );

                        ( await mExportManager.ExportProject( exportParameters ))
                            .IfLeft( error => mLog.LogError( error ));
                    }
                });
            }
        }

        public void Dispose() {
            mStateSubscription.Dispose();
        }
    }
}
