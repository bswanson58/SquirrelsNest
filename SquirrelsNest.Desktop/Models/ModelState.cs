using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Desktop.Models {
    internal class ModelState : IModelState {
        private readonly Subject<CurrentState>  mCurrentState;
        private Option<SnProject>               mCurrentProject;

        public IObservable<CurrentState>        OnStateChange => mCurrentState.AsObservable();

        public ModelState() {
            mCurrentState = new Subject<CurrentState>();
            mCurrentProject = Option<SnProject>.None;
        }

        public void SetProject( SnProject project ) {
            mCurrentProject = project;

            NotifyState();
        }

        public void ClearProject() {
            mCurrentProject = Option<SnProject>.None;

            NotifyState();
        }

        private void NotifyState() {
            mCurrentState.OnNext( new CurrentState( mCurrentProject ));
        }
    }
}
