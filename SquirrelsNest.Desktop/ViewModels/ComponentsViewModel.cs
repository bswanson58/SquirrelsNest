﻿using System;
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
    internal class ComponentsViewModel : ObservableObject, IDisposable {
        private readonly IComponentProvider mComponentProvider;
        private readonly IDialogService     mDialogService;
        private readonly IDisposable        mStateSubscription;
        private readonly ILog               mLog;
        private SnProject ?                 mCurrentProject;

        public  ObservableCollection<SnComponent>   ComponentList { get; }

        public  IRelayCommand                       CreateComponent { get; }

        public ComponentsViewModel( IComponentProvider componentProvider, IModelState modelState, IDialogService dialogService, ILog log ) {
            mComponentProvider = componentProvider;
            mDialogService = dialogService;
            mLog = log;

            ComponentList = new ObservableCollection<SnComponent>();
            CreateComponent = new RelayCommand( OnCreateRelease );

            mStateSubscription = modelState.OnStateChange.Subscribe( OnStateChanged );
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project.Do( project => {
                LoadComponents( project );

                mCurrentProject = project;
            });
        }

        private void LoadComponents( SnProject forProject ) {
            ComponentList.Clear();

            mComponentProvider
                .GetComponents( forProject ).Result
                .Match( list => list.ForEach( p => ComponentList.Add( p )),
                        error => mLog.LogError( error ));
        }

        private void OnCreateRelease() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters();

                mDialogService.ShowDialog( nameof( EditComponentDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var component = result.Parameters.GetValue<SnComponent>( EditComponentDialogViewModel.cComponentParameter );

                        if( component == null ) throw new ApplicationException( "SnComponent was not returned when editing component" );

                        mComponentProvider
                            .AddComponent( component.For( mCurrentProject )).Result
                            .Match( _ => LoadComponents( mCurrentProject ),
                                    error => mLog.LogError( error ));
                    }
                });
            }
        }

        public void Dispose() {
            mComponentProvider.Dispose();
            mStateSubscription.Dispose();
        }
    }
}