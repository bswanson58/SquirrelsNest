using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using LanguageExt;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Preferences;

namespace SquirrelsNest.Desktop.Models {
    internal class ModelState : IModelState {
        private readonly IProjectProvider               mProjectProvider;
        private readonly IUserData                      mUserData;
        private readonly ILog                           mLog;
        private readonly BehaviorSubject<CurrentState>  mModelState;
        private Option<SnProject>                       mCurrentProject;
        private Option<SnUser>                          mCurrentUser;

        public  IObservable<CurrentState>               OnStateChange => mModelState.AsObservable();

        public  CurrentState                            CurrentState => new CurrentState( mCurrentProject, mCurrentUser );

        public ModelState( IProjectProvider projectProvider, IUserData userData, ILog log ) {
            mProjectProvider = projectProvider;
            mUserData = userData;
            mLog = log;
            mCurrentProject = Option<SnProject>.None;
            mCurrentUser = SnUser.Default;

            mModelState = new BehaviorSubject<CurrentState>( CurrentState );
        }

        public async Task SetUser( SnUser user ) {
            mCurrentUser = user;

            NotifyStateChange();

            await EstablishBestProject( user );
        }

        public async Task SetProject( SnProject project ) {
            mCurrentProject = project;

            await SetLastProject();

            NotifyStateChange();
        }

        private async Task SetLastProject() {
            await mCurrentProject.IfSomeAsync( project => {
                mCurrentUser.IfSomeAsync( async user => {
                    await mUserData.Save( user, UserDataType.LastProject, new UserProjectPreference( project.EntityId ));
                });
            });
        }

        private async Task EstablishBestProject( SnUser forUser ) {
            var projectList = await mProjectProvider.GetProjects( forUser );
            var lastProjectId = 
                ( await mUserData.Load<UserProjectPreference>( forUser, UserDataType.LastProject ))
                .Map( data => EntityId.For( data.LastProjectId ).IfNone( EntityId.Default ));

            var lastProject =
                from projectId in lastProjectId
                from projects in projectList
                select projects.FirstOrDefault( p => p.EntityId.Equals( projectId ), projects.FirstOrDefault( SnProject.Default ));

            ( await lastProject.MapAsync( 
                    async project => {
                        if(!project.Equals( SnProject.Default )) {
                            await SetProject( project );
                        }

                        return Unit.Default;
                    }))
                    .IfLeft( error => mLog.LogError( error ));
        }

        private void NotifyStateChange() {
            mModelState.OnNext( CurrentState );
        }
    }
}
