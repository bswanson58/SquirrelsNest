using System;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Toolkit.Mvvm.Input;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.ProjectTemplates;
using SquirrelsNest.Desktop.Models;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ProjectPropertiesViewModel : IDisposable {
        private readonly IProjectTemplateManager    mTemplateManager;
        private readonly IDisposable                mStateSubscription;
        private readonly ILog                       mLog;
        private SnProject ?                         mCurrentProject;

        public  IRelayCommand                       CreateTemplate { get; }

        public ProjectPropertiesViewModel( IModelState modelState, IProjectTemplateManager templateManager, ILog log,
                                           SynchronizationContext context ) {
            mLog = log;
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
                var templateParameters = new TemplateParameters{ TemplateName = "My Template", TemplateDescription = "Some description" };

                mTemplateManager.CreateTemplate( mCurrentProject, templateParameters )
                    .IfLeft( error => mLog.LogError( error ));
            }
        }

        public void Dispose() {
            mStateSubscription.Dispose();
        }
    }
}
