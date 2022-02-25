using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Models;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class CurrentUserViewModel : ObservableObject, IDisposable {
        private readonly IDisposable    mUserChangedSubscription;
        private SnUser                  mCurrentUser;

        public  string                  UserName => mCurrentUser.Name;

        public CurrentUserViewModel( IModelState modelState ) {
            mCurrentUser = SnUser.Default;

            mUserChangedSubscription = modelState.OnUserChange.Subscribe( OnUserChanged );
        }

        private void OnUserChanged( CurrentState state ) {
            mCurrentUser = state.User;

            OnPropertyChanged( nameof( UserName ));
        }

        public void Dispose() {
            mUserChangedSubscription.Dispose();
        }
    }
}
