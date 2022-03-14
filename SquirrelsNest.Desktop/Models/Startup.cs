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
        private readonly IUserProvider          mUserProvider;
        private readonly IModelState            mModelState;
        private readonly IApplicationLog        mLog;
        private readonly IPreferences<AppState> mAppState;
        private Action                          mClosingAction;

        public Startup( IModelState modelState, IUserProvider userProvider, IPreferences<AppState> appState, IApplicationLog log ) {
            mModelState = modelState;
            mLog = log;
            mUserProvider = userProvider;
            mAppState = appState;
            mClosingAction = () => { };
        }

        public async void StartApplication( Action onClose ) {
            mClosingAction = onClose;

            mLog.ApplicationStarting();

            await EstablishCurrentUser( mAppState );

            ShowMainWindow( OnClosing );
        }

        private void OnClosing() {
            mModelState.CurrentState.Project
                .Do( project => mAppState.Save( mAppState.Current.With( projectId: project.EntityId )));

            mClosingAction();
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

            ( await bestUser.MapAsync( 
                async user => {
                    await mModelState.SetUser( user );

                    if(!user.EntityId.Value.Equals( currentState.UserId )) {
                        mAppState.Save( currentState.With( userId: user.EntityId ));
                    }

                    return Unit.Default;
                }))
                .IfLeft( error => mLog.LogError( error ));
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
