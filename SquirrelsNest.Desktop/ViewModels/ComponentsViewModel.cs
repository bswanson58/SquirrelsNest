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
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  IRelayCommand<SnComponent>          EditComponent { get; }
        public  IRelayCommand<SnComponent>          DeleteComponent { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public ComponentsViewModel( IComponentProvider componentProvider, IModelState modelState, IDialogService dialogService, ILog log ) {
            mComponentProvider = componentProvider;
            mDialogService = dialogService;
            mLog = log;

            ComponentList = new ObservableCollection<SnComponent>();
            CreateComponent = new RelayCommand( OnCreateRelease );
            EditComponent = new RelayCommand<SnComponent>( OnEditComponent );
            DeleteComponent = new RelayCommand<SnComponent>( OnDeleteComponent );

            mStateSubscription = modelState.OnStateChange.SubscribeAsync( OnStateChanged, OnError );
        }

        private async Task<Unit> OnStateChanged( CurrentState state ) {
            state.Project.Do( project => {
                mCurrentProject = project;
            });

            await LoadComponents();

            return Unit.Default;
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During ModelStateChanged in {nameof( ComponentsViewModel )}", ex );
        }

        private async Task LoadComponents() {
            if( mCurrentProject != null ) {
                ComponentList.Clear();

                ( await mComponentProvider.GetComponents( mCurrentProject ))
                    .Match( list => list.ForEach( p => ComponentList.Add( p )),
                            error => mLog.LogError( error ));
            }
        }

        private void OnCreateRelease() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters();

                mDialogService.ShowDialog( nameof( EditComponentDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var component = result.Parameters.GetValue<SnComponent>( EditComponentDialogViewModel.cComponentParameter );

                        if( component == null ) throw new ApplicationException( "SnComponent was not returned when editing component" );

                        ( await mComponentProvider.AddComponent( component.For( mCurrentProject )))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadComponents();
                    }
                });
            }
        }

        private void OnEditComponent( SnComponent ? editComponent ) {
            if(( mCurrentProject != null ) &&
               ( editComponent != null )) {
                var parameters = new DialogParameters {{ EditComponentDialogViewModel.cComponentParameter, editComponent }};

                mDialogService.ShowDialog( nameof( EditComponentDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var component = result.Parameters.GetValue<SnComponent>( EditComponentDialogViewModel.cComponentParameter );

                        if( component == null ) throw new ApplicationException( "SnComponent was not returned when editing component" );

                        ( await mComponentProvider.UpdateComponent( component.For( mCurrentProject )))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadComponents();
                    }
                });
            }
        }

        private void OnDeleteComponent( SnComponent ? component ) {
            if( component != null ) {
                var parameters = new DialogParameters {
                    { ConfirmationDialogViewModel.cConfirmationText, $"Would you like to delete component named '{component.Name}'?" }
                };

                mDialogService.ShowDialog( nameof( ConfirmationDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        ( await mComponentProvider.DeleteComponent( component ))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadComponents();
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
