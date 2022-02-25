using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.Models {
    internal class ModelState : IModelState {
        private readonly BehaviorSubject<CurrentState>  mProjectState;
        private readonly BehaviorSubject<CurrentState>  mUserState;
        private Option<SnProject>                       mCurrentProject;
        private SnUser                                  mCurrentUser;

        public IObservable<CurrentState>                OnStateChange => mProjectState.AsObservable();
        public IObservable<CurrentState>                OnUserChange => mUserState.AsObservable();

        public CurrentState                             CurrentState => ConstructState;

        public ModelState() {
            mCurrentProject = Option<SnProject>.None;
            mCurrentUser = SnUser.Default;

            mProjectState = new BehaviorSubject<CurrentState>( ConstructState );
            mUserState = new BehaviorSubject<CurrentState>( ConstructState );
        }

        public void SetProject( SnProject project ) {
            mCurrentProject = project;

            NotifyProjectState();
        }

        public void ClearProject() {
            mCurrentProject = Option<SnProject>.None;

            NotifyProjectState();
        }

        public void SetUser( SnUser user ) {
            mCurrentUser = user;

            NotifyUserState();
        }

        public void ClearUser() {
            mCurrentUser = SnUser.Default;

            NotifyUserState();
        }

        private CurrentState ConstructState => new CurrentState( mCurrentProject, mCurrentUser );

        private void NotifyProjectState() {
            mProjectState.OnNext( ConstructState );
        }

        private void NotifyUserState() {
            mUserState.OnNext( ConstructState );
        }
    }
}
