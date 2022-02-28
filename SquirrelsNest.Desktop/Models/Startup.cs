using System;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Desktop.Preferences;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.Models {
    internal class Startup {
        private readonly IProjectProvider       mProjectProvider;
        private readonly IUserProvider          mUserProvider;
        private readonly IModelState            mModelState;
        private readonly IApplicationLog        mLog;
        private readonly IPreferences<AppState> mAppState;
        private Action                          mClosingAction;

        public Startup( IModelState modelState, IProjectProvider projectProvider, IUserProvider userProvider, IPreferences<AppState> appState, IApplicationLog log ) {
            mModelState = modelState;
            mLog = log;
            mProjectProvider = projectProvider;
            mUserProvider = userProvider;
            mAppState = appState;
            mClosingAction = () => { };
        }

        public void StartApplication( Action onClose ) {
            mClosingAction = onClose;

            mLog.ApplicationStarting();

            EstablishCurrentUser( mAppState );
            EstablishCurrentProject( mAppState );

            ShowMainWindow( OnClosing );
        }

        private void OnClosing() {
            mModelState.CurrentState.Project
                .Do( project => mAppState.Save( mAppState.Current.With( projectId: project.EntityId )));

            mClosingAction();
        }

        private void EstablishCurrentProject( IPreferences<AppState> appState ) {
            var currentState = appState.Current;

            if( currentState.ProjectId != EntityId.Default ) {
                EntityId.For( currentState.ProjectId )
                    .Bind( projectId => mProjectProvider.GetProject( projectId ).Result )
                    .Iter( p => mModelState.SetProject( p.Right ));
            }
            else {
                mProjectProvider.GetProjects().Result
                    .Match( list => {
                            var firstProject = list.FirstOrDefault( SnProject.Default );

                            if(!firstProject.EntityId.Equals( EntityId.Default )) {
                                mModelState.SetProject( firstProject );
                            }
                        },
                        error => mLog.LogError( error ));
            }
        }

        private void EstablishCurrentUser( IPreferences<AppState> appState ) {
            var currentState = appState.Current;
            var currentUserId = EntityId.For( currentState.UserId );
            var bestUser =
                from currentUser in GetUserOrDefault( currentUserId )
                from firstUser in GetFirstUser()
                select currentUser.EntityId.Equals( EntityId.Default ) ? firstUser : currentUser;

            bestUser.Do( 
                user => {
                    mModelState.SetUser( user );

                    if(!user.EntityId.Value.Equals( currentState.UserId )) {
                        mAppState.Save( currentState.With( userId: user.EntityId ));
                    }
                });
        }

        private Either<Error, SnUser> GetUserOrDefault( Option<EntityId> userId ) {
            var found = userId.Match( 
                    Some: id => mUserProvider.GetUser( id ).Result,
                    None: SnUser.Default );

            if( found.IsLeft ) {
                return SnUser.Default;
            }

            return found;
        }

        private Either<Error, SnUser> GetFirstUser() {
            return mUserProvider
                .GetUsers().Result
                .Map( list => list.FirstOrDefault( SnUser.Default ));
        }

        private void ShowMainWindow( Action onClose ) {
            var window = new MainWindow();

            window.Closed += (_, _) => {
                onClose();

                mLog.ApplicationExiting();
            };

            window.Show();
        }
    }
}
