using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;

namespace SquirrelsNest.Core.Tests.Database {
    public class BaseProviderTests : IDisposable {
        protected readonly IDbIssueProvider         mIssueProvider;
        protected readonly IDbComponentProvider     mComponentProvider;
        protected readonly IDbIssueTypeProvider     mIssueTypeProvider;
        protected readonly IDbReleaseProvider       mReleaseProvider;
        protected readonly IDbWorkflowStateProvider mStateProvider;
        protected readonly IDbUserProvider          mUserProvider;
        protected readonly IDbProjectProvider       mProjectProvider;

        protected BaseProviderTests() {
            var contextFactory = new EfContextFactory();

            mIssueProvider = new EfDb.Providers.IssueProvider( contextFactory );
            mComponentProvider = new EfDb.Providers.ComponentProvider( contextFactory );
            mIssueTypeProvider = new EfDb.Providers.IssueTypeProvider( contextFactory );
            mReleaseProvider = new EfDb.Providers.ReleaseProvider( contextFactory );
            mStateProvider = new EfDb.Providers.WorkflowStateProvider( contextFactory );
            mUserProvider = new EfDb.Providers.UserProvider( contextFactory );
            mProjectProvider = new EfDb.Providers.ProjectProvider( contextFactory );
        }

        protected async Task<SnIssue[]> CreateSomeIssues( uint count, SnProject forProject ) {
            var retValue = new List<SnIssue>();

            for( uint index = 0; index < count; index++ ) {
                retValue.Add( new SnIssue( $"issue {index + 1}", index + 1, forProject.EntityId ));
            }

            return await CreateSomeIssues( retValue.ToArray());
        }

        protected async Task<SnIssue[]> CreateSomeIssues( uint count, SnComponent[] components ) {
            var retValue = new List<SnIssue>();

            for( uint index = 0; index < components.Length; index++ ) {
                retValue.Add( new SnIssue( $"issue {index + 1}", index + 1, SnProject.Default.EntityId ).With( components[index]));
            }

            return await CreateSomeIssues( retValue.ToArray());
        }

        protected async Task<SnIssue[]> CreateSomeIssues( uint count, SnIssueType[] issueTypes ) {
            var retValue = new List<SnIssue>();

            for( uint index = 0; index < issueTypes.Length; index++ ) {
                retValue.Add( new SnIssue( $"issue {index + 1}", index + 1, SnProject.Default.EntityId ).With( issueTypes[index]));
            }

            return await CreateSomeIssues( retValue.ToArray());
        }

        protected async Task<SnIssue[]> CreateSomeIssues( uint count, SnRelease[] releases ) {
            var retValue = new List<SnIssue>();

            for( uint index = 0; index < releases.Length; index++ ) {
                retValue.Add( new SnIssue( $"issue {index + 1}", index + 1, SnProject.Default.EntityId ).With( releases[index]));
            }

            return await CreateSomeIssues( retValue.ToArray());
        }

        protected async Task<SnIssue[]> CreateSomeIssues( uint count, SnWorkflowState[] states ) {
            var retValue = new List<SnIssue>();

            for( uint index = 0; index < states.Length; index++ ) {
                retValue.Add( new SnIssue( $"issue {index + 1}", index + 1, SnProject.Default.EntityId ).With( states[index]));
            }

            return await CreateSomeIssues( retValue.ToArray());
        }

        protected async Task<SnIssue[]> CreateSomeIssues( uint count, SnUser[] enteredBy, SnUser[] assignedTo ) {
            var retValue = new List<SnIssue>();

            if( enteredBy.Length != assignedTo.Length ) throw new ArgumentException( "enteredBy and assignedTo users must be provided" );

            for( uint index = 0; index < enteredBy.Length; index++ ) {
                retValue.Add( new SnIssue( $"issue {index + 1}", index + 1, SnProject.Default.EntityId )
                    .With( enteredBy: enteredBy[index].EntityId, assignedTo: assignedTo[index].EntityId ));
            }

            return await CreateSomeIssues( retValue.ToArray());
        }

        private async Task<SnIssue[]> CreateSomeIssues( SnIssue[] issues ) {
            var retValue = new List<SnIssue>();

            foreach( var issue in issues ) {
                (await mIssueProvider.AddIssue( issue ))
                    .Do( i => retValue.Add( i ));
            }

            return retValue.ToArray();
        }

        protected async Task<SnComponent[]> CreateSomeComponents( uint count, SnProject ? forProject = null ) {
            var retValue = new List<SnComponent>();
            var project = forProject ?? SnProject.Default;

            for( var index = 0; index < count; index++ ) {
                (await mComponentProvider.AddComponent( new SnComponent( $"component {index + 1}" ).For( project )))
                    .Do( c => retValue.Add( c ));
            }

            return retValue.ToArray();
        }

        protected async Task<SnIssueType[]> CreateSomeIssueTypes( uint count, SnProject ? forProject = null ) {
            var retValue = new List<SnIssueType>();
            var project = forProject ?? SnProject.Default;

            for( var index = 0; index < count; index++ ) {
                (await mIssueTypeProvider.AddIssue( new SnIssueType( $"issue type {index + 1}" ).For( project )))
                    .Do( c => retValue.Add( c ));
            }

            return retValue.ToArray();
        }

        protected async Task<SnRelease[]> CreateSomeReleases( uint count, SnProject ? forProject = null ) {
            var retValue = new List<SnRelease>();
            var project = forProject ?? SnProject.Default;

            for( var index = 0; index < count; index++ ) {
                (await mReleaseProvider.AddRelease( new SnRelease( $"release {index + 1}" ).For( project )))
                    .Do( c => retValue.Add( c ));
            }

            return retValue.ToArray();
        }

        protected async Task<SnWorkflowState[]> CreateSomeStates( uint count, SnProject ? forProject = null ) {
            var retValue = new List<SnWorkflowState>();
            var project = forProject ?? SnProject.Default;

            for( var index = 0; index < count; index++ ) {
                (await mStateProvider.AddState( new SnWorkflowState( $"state {index + 1}" ).For( project )))
                    .Do( c => retValue.Add( c ));
            }

            return retValue.ToArray();
        }

        protected async Task<SnUser[]> CreateSomeUsers( uint count ) {
            var retValue = new List<SnUser>();

            for( var index = 0; index < count; index++ ) {
                (await mUserProvider.AddUser( new SnUser( $"user {index + 1}", $"user{index}@bob.com" )))
                    .Do( c => retValue.Add( c ));
            }

            return retValue.ToArray();
        }

        protected async Task<SnProject[]> CreateSomeProjects( uint count ) {
            var retValue = new List<SnProject>();

            for( var index = 0; index < count; index++ ) {
                (await mProjectProvider.AddProject( new SnProject( $"project {index + 1}", $"P{index}" )))
                    .Do( c => retValue.Add( c ));
            }

            return retValue.ToArray();
        }

        private void DeleteDatabase() {
            var factory = new EfContextFactory();

            factory.ProvideContext().Database.EnsureDeleted();
        }

        public virtual void Dispose() {
            mIssueProvider.Dispose();
            mComponentProvider.Dispose();
            mIssueTypeProvider.Dispose();
            mReleaseProvider.Dispose();
            mStateProvider.Dispose();
            mUserProvider.Dispose();
            mProjectProvider.Dispose();

            DeleteDatabase();
        }
    }
}
