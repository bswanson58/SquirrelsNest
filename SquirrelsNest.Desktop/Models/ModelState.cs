using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.Models {
    internal class ModelState : IModelState {
        private readonly BehaviorSubject<CurrentState>  mModelState;
        private Option<SnProject>                       mCurrentProject;
        private SnUser                                  mCurrentUser;

        public  IObservable<CurrentState>               OnStateChange => mModelState.AsObservable();

        public  CurrentState                            CurrentState => new CurrentState( mCurrentProject, mCurrentUser );

        public ModelState() {
            mCurrentProject = Option<SnProject>.None;
            mCurrentUser = SnUser.Default;

            mModelState = new BehaviorSubject<CurrentState>( CurrentState );
        }

        public void SetProject( SnProject project ) {
            mCurrentProject = project;

            NotifyStateChange();
        }

        public void ClearProject() {
            mCurrentProject = Option<SnProject>.None;

            NotifyStateChange();
        }

        public void SetUser( SnUser user ) {
            mCurrentUser = user;

            NotifyStateChange();
        }

        public void ClearUser() {
            mCurrentUser = SnUser.Default;

            NotifyStateChange();
        }

        private void NotifyStateChange() {
            mModelState.OnNext( CurrentState );
        }
    }
}
