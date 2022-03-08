using System;
using Gravatar;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Models;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class CurrentUserViewModel : ObservableObject, IDisposable {
        private readonly IDisposable        mUserChangedSubscription;
        private readonly IGravatarClient    mGravatarClient;
        private SnUser                      mCurrentUser;

        public  string                      UserName => mCurrentUser.Name;
        public  byte[]                      UserImage { get; private  set; }

        public CurrentUserViewModel( IModelState modelState, IGravatarClient gravatarClient ) {
            mGravatarClient = gravatarClient;
            mCurrentUser = SnUser.Default;

            mUserChangedSubscription = modelState.OnStateChange.Subscribe( OnUserChanged );
            UserImage = Array.Empty<byte>();
        }

        private void OnUserChanged( CurrentState state ) {
            mCurrentUser = state.User;

            LoadUserProfile();
            OnPropertyChanged( nameof( UserName ));
        }

        private async void LoadUserProfile() {
            if(!String.IsNullOrWhiteSpace( mCurrentUser.Email )) {
                await using var stream = await mGravatarClient.GetImage( mCurrentUser.Email, GravatarDefaultImage.IdentIcon, false, 100 );

                UserImage = stream.ToArray();
                OnPropertyChanged( nameof( UserImage ));
            }
        }

        public void Dispose() {
            mUserChangedSubscription.Dispose();
        }
    }
}
