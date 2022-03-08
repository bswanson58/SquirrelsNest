using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async void StartApplication( Action onClose ) {
            mClosingAction = onClose;

            mLog.ApplicationStarting();

            await EstablishCurrentUser( mAppState );
            await EstablishCurrentProject( mAppState );

            ShowMainWindow( OnClosing );
        }

        private void OnClosing() {
            mModelState.CurrentState.Project
                .Do( project => mAppState.Save( mAppState.Current.With( projectId: project.EntityId )));

            mClosingAction();
        }

        private async Task EstablishCurrentProject( IPreferences<AppState> appState ) {
            var currentState = appState.Current;

            if( currentState.ProjectId != EntityId.Default ) {
                var projectId = EntityId.For( currentState.ProjectId );
                var project = await projectId.MapAsync( async p => await mProjectProvider.GetProject( p ));

                project.Match( 
                    p => mModelState.SetProject( p ),
                    error => mLog.LogError( error ));
            }
            else {
                var projects = await  mProjectProvider.GetProjects();
                
                projects
                    .Match( list => {
                            var firstProject = list.FirstOrDefault( SnProject.Default );

                            if(!firstProject.EntityId.Equals( EntityId.Default )) {
                                mModelState.SetProject( firstProject );
                            }
                        },
                        error => mLog.LogError( error ));
            }
        }

        private async Task EstablishCurrentUser( IPreferences<AppState> appState ) {
            var currentState = appState.Current;
            var currentUserId = EntityId.For( currentState.UserId );
            var currentUser = await GetUserOrDefault( currentUserId );
            var firstUser = await GetFirstUser();

            var bestUser =
                from c in currentUser
                from f in firstUser
                select c.EntityId.Equals( EntityId.Default ) ? f : c;

            bestUser.Do( 
                user => {
                    mModelState.SetUser( user );

                    if(!user.EntityId.Value.Equals( currentState.UserId )) {
                        mAppState.Save( currentState.With( userId: user.EntityId ));
                    }
                });
        }

        private async Task<Either<Error, SnUser>> GetUserOrDefault( Option<EntityId> userId ) {
            var dbUser = await userId.MapAsync( async id => await mUserProvider.GetUser( id ));

            return dbUser.IfLeft( SnUser.Default );
        }

        private async Task<Either<Error, SnUser>> GetFirstUser() {
            var userList = await mUserProvider.GetUsers();

            return userList.Map( list => list.FirstOrDefault( SnUser.Default ));
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
