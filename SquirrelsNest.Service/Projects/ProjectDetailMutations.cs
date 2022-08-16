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
        public async Task<AddProjectDetailPayload> AddProjectDetail( AddProjectDetailInput detailInput ) {
            var user = await GetUser();

            if( user.IsLeft ) {
                return new AddProjectDetailPayload( "The user could not be determined" );
            }

            var projectId = EntityId.For( detailInput.ProjectId );
            if( projectId.IsNone ) {
                return new AddProjectDetailPayload( "Invalid project ID to be updated" );
            }

            var project = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));

            if( project.IsLeft ) {
                return new AddProjectDetailPayload( "Project could not be loaded" );
            }

            foreach( var component in detailInput.Components ) {
                var result = await mComponentProvider.AddComponent( component.ToEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new AddProjectDetailPayload( String.Empty ), e => new AddProjectDetailPayload( e ) );
                }
            }

            foreach( var issueType in detailInput.IssueTypes ) {
                var result = await mIssueTypeProvider.AddIssue( issueType.ToEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new AddProjectDetailPayload( String.Empty ), e => new AddProjectDetailPayload( e ) );
                }
            }

            foreach( var state in detailInput.States ) {
                var result = await mStateProvider.AddState( state.ToEntity());

                if( result.IsLeft ) {
                    return result.Match( _ => new AddProjectDetailPayload( String.Empty ), e => new AddProjectDetailPayload( e ) );
                }
            }

            var compositeProject = await project.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));

            return compositeProject.Match( p => new AddProjectDetailPayload( p.ToCl()), e => new AddProjectDetailPayload( e ));
        }

    }
}
