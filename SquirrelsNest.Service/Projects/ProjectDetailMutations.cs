using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Dto.Mutations;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Projects {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class ProjectDetailMutations {
        private readonly IProjectProvider       mProjectProvider;
        private readonly IProjectBuilder        mProjectBuilder;
        private readonly IComponentProvider     mComponentProvider;
        private readonly IIssueTypeProvider     mIssueTypeProvider;
        private readonly IWorkflowStateProvider mStateProvider;
        private readonly IUserProvider          mUserProvider;

        public ProjectDetailMutations( IProjectProvider projectProvider, IProjectBuilder projectBuilder,
                                       IComponentProvider componentProvider, IIssueTypeProvider issueTypeProvider,
                                       IWorkflowStateProvider stateProvider, IUserProvider userProvider ) {
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mStateProvider = stateProvider;
            mUserProvider = userProvider;
        }

        private async Task<Either<Error, SnUser>> GetUser() {
            var users = await mUserProvider.GetUsers();

            return users.Map( userList => userList.FirstOrDefault( SnUser.Default ));
        }

        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<ProjectDetailPayload> AddProjectDetail( ProjectDetailInput detailInput ) {
            var user = await GetUser();

            if( user.IsLeft ) {
                return new ProjectDetailPayload( "The user could not be determined" );
            }

            var projectId = EntityId.For( detailInput.ProjectId );
            if( projectId.IsNone ) {
                return new ProjectDetailPayload( "Invalid project ID to be updated" );
            }

            var project = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));

            if( project.IsLeft ) {
                return new ProjectDetailPayload( "Project could not be loaded" );
            }

            foreach( var component in detailInput.Components ) {
                var result = await mComponentProvider.AddComponent( component.ToNewEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            foreach( var issueType in detailInput.IssueTypes ) {
                var result = await mIssueTypeProvider.AddIssue( issueType.ToNewEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            foreach( var state in detailInput.States ) {
                var result = await mStateProvider.AddState( state.ToNewEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            var compositeProject = await project.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));

            return compositeProject.Match( p => new ProjectDetailPayload( p.ToCl()), e => new ProjectDetailPayload( e ));
        }

        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<ProjectDetailPayload> DeleteProjectDetail( ProjectDetailInput detailInput ) {
            var user = await GetUser();

            if( user.IsLeft ) {
                return new ProjectDetailPayload( "The user could not be determined" );
            }

            var projectId = EntityId.For( detailInput.ProjectId );
            if( projectId.IsNone ) {
                return new ProjectDetailPayload( "Invalid project ID to be updated" );
            }

            var project = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));

            if( project.IsLeft ) {
                return new ProjectDetailPayload( "Project could not be loaded" );
            }
            foreach( var component in detailInput.Components ) {
                var entityId = EntityId.For( component.Id );
                var existing = await entityId.MapAsync( id => mComponentProvider.GetComponent( id ));
                var result = await existing.BindAsync( c => mComponentProvider.DeleteComponent( c ));

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            foreach( var issueType in detailInput.IssueTypes ) {
                var entityId = EntityId.For( issueType.Id );
                var existing = await entityId.MapAsync( id => mIssueTypeProvider.GetIssue( id ));
                var result = await existing.BindAsync( i => mIssueTypeProvider.DeleteIssue( i ));

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            foreach( var state in detailInput.States ) {
                var entityId = EntityId.For( state.Id );
                var existing = await entityId.MapAsync( id => mStateProvider.GetState( id ));
                var result = await existing.BindAsync( s => mStateProvider.DeleteState( s ));

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            var compositeProject = await project.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));

            return compositeProject.Match( p => new ProjectDetailPayload( p.ToCl()), e => new ProjectDetailPayload( e ));
        }    

        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<ProjectDetailPayload> UpdateProjectDetail( ProjectDetailInput detailInput ) {
            var user = await GetUser();

            if( user.IsLeft ) {
                return new ProjectDetailPayload( "The user could not be determined" );
            }

            var projectId = EntityId.For( detailInput.ProjectId );
            if( projectId.IsNone ) {
                return new ProjectDetailPayload( "Invalid project ID to be updated" );
            }

            var project = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));

            if( project.IsLeft ) {
                return new ProjectDetailPayload( "Project could not be loaded" );
            }
            foreach( var component in detailInput.Components ) {
                var result = await mComponentProvider.UpdateComponent( component.ToEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            foreach( var issueType in detailInput.IssueTypes ) {
                var result = await mIssueTypeProvider.UpdateIssue( issueType.ToEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            foreach( var state in detailInput.States ) {
                var result = await mStateProvider.UpdateState( state.ToEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new ProjectDetailPayload( String.Empty ), e => new ProjectDetailPayload( e ) );
                }
            }

            var compositeProject = await project.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));

            return compositeProject.Match( p => new ProjectDetailPayload( p.ToCl()), e => new ProjectDetailPayload( e ));
        }    
    }
}
