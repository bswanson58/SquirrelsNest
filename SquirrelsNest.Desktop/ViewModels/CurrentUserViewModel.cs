using System;
using System.Threading.Tasks;
using Gravatar;
using LanguageExt;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Extensions;
using SquirrelsNest.Desktop.Models;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class CurrentUserViewModel : ObservableObject, IDisposable {
        private readonly IDisposable        mUserChangedSubscription;
        private readonly IGravatarClient    mGravatarClient;
        private readonly ILog               mLog;
        private Option<SnUser>              mCurrentUser;

        public  string                      UserName { get; private set; }
        public  byte[]                      UserImage { get; private  set; }

        public CurrentUserViewModel( IModelState modelState, IGravatarClient gravatarClient, ILog log ) {
            mGravatarClient = gravatarClient;
            mLog = log;
            mCurrentUser = SnUser.Default;

            mUserChangedSubscription = modelState.OnStateChange.SubscribeAsync( OnUserChanged, OnError );
            UserImage = Array.Empty<byte>();
            UserName = String.Empty;
        }

        private async Task OnUserChanged( CurrentState state ) {
            mCurrentUser = state.User;

            await mCurrentUser.MapAsync( LoadUserProfile );

            if( mCurrentUser.IsNone ) {
                UserName = String.Empty;
                UserImage = Array.Empty<byte>();

                OnPropertyChanged( nameof( UserImage ));
                OnPropertyChanged( nameof( UserName ));
            }
        }

        private void OnError( Exception ex ) {
            mLog.LogException( "ModelState OnStateChange error", ex );
        }

        private async Task LoadUserProfile( SnUser user ) {
            if(!String.IsNullOrWhiteSpace( user.Email )) {
                await using var stream = await mGravatarClient.GetImage( user.Email, GravatarDefaultImage.IdentIcon, false, 100 );

                UserImage = stream.ToArray();
                UserName = user.Name;

                OnPropertyChanged( nameof( UserImage ));
                OnPropertyChanged( nameof( UserName ));
            }
        }

        public void Dispose() {
            mUserChangedSubscription.Dispose();
        }
    }
}
