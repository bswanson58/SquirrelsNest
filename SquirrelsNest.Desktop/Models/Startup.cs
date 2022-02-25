using System;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Desktop.Preferences;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.Models {
    internal class Startup {
        private readonly IUserProvider          mUserProvider;
        private readonly IModelState            mModelState;
        private readonly IApplicationLog        mLog;
        private readonly IPreferences<AppState> mAppState;

        public Startup( IModelState modelState, IUserProvider userProvider, IPreferences<AppState> appState, IApplicationLog log ) {
            mModelState = modelState;
            mLog = log;
            mUserProvider = userProvider;
            mAppState = appState;
        }

        public void StartApplication( Action onClose ) {
            mLog.ApplicationStarting();

            EstablishCurrentUser( mAppState );

            ShowMainWindow( onClose );
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
