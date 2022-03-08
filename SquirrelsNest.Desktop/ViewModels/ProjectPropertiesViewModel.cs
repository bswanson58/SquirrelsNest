using System;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.ProjectTemplates;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ProjectPropertiesViewModel : IDisposable {
        private readonly IProjectTemplateManager    mTemplateManager;
        private readonly IDialogService             mDialogService;
        private readonly IDisposable                mStateSubscription;
        private readonly ILog                       mLog;
        private SnProject ?                         mCurrentProject;

        public  IRelayCommand                       CreateTemplate { get; }

        public ProjectPropertiesViewModel( IModelState modelState, IProjectTemplateManager templateManager, IDialogService dialogService,
                                           ILog log, SynchronizationContext context ) {
            mLog = log;
            mDialogService = dialogService;
            mTemplateManager = templateManager;

            CreateTemplate = new RelayCommand( OnCreateTemplate );

            mStateSubscription = modelState.OnStateChange.ObserveOn( context ).Subscribe( OnStateChanged );
        }

        private void OnStateChanged( CurrentState state ) {
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

        public void Dispose() {
            mStateSubscription.Dispose();
        }
    }
}
