using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
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
    public class ProjectMutations : BaseGraphProvider {
        private readonly IProjectProvider       mProjectProvider;
        private readonly IProjectBuilder        mProjectBuilder;

        public ProjectMutations( IUserProvider userProvider, IProjectProvider projectProvider, IProjectBuilder projectBuilder,
                                 IHttpContextAccessor contextAccessor, IApplicationLog log ) :
            base( userProvider, contextAccessor, log ){
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
        }

        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<AddProjectPayload> AddProject( AddProjectInput projectInput ) {
            var user = await GetUser();

            if( user.IsLeft ) {
                return new AddProjectPayload( "The user could not be determined" );
            }

            var newProject = ( new SnProject( projectInput.Title, projectInput.IssuePrefix )).With( description: projectInput.Description );
            var addedProject = await user.BindAsync( u => mProjectProvider.AddProject( newProject, u ));
            var compositeProject = await addedProject.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));

            return compositeProject.Match( p => new AddProjectPayload( p.ToCl()), e => new AddProjectPayload( e ));
        }

        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<UpdateProjectPayload> UpdateProject( UpdateProjectInput updateInput ) {
            var projectId = EntityId.For( updateInput.ProjectId );
            if( projectId.IsNone ) {
                return new UpdateProjectPayload( "Invalid project ID to be updated" );
            }

            var currentProject = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));
            var updatedProject = currentProject.Map( project => 
                project.With( name: updateInput.Title, description: updateInput.Description, issuePrefix: updateInput.IssuePrefix ));
            var updateResult = await updatedProject.BindAsync( p => mProjectProvider.UpdateProject( p ));

            if( updateResult.IsLeft ) {
                return new UpdateProjectPayload( updateResult.LeftToList().FirstOrDefault());
            }

            var compositeProject = await updatedProject.BindAsync( p => mProjectBuilder.BuildCompositeProject( p ));

            return compositeProject.Match( p => new UpdateProjectPayload( p.ToCl() ), e => new UpdateProjectPayload( e ));
        }

        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<DeleteProjectPayload> DeleteProject( DeleteProjectInput deleteInput ) {
            var user = await GetUser();

            if( user.IsLeft ) {
                return new DeleteProjectPayload( "The user could not be determined" );
            }

            var projectId = EntityId.For( deleteInput.ProjectId );
            if( projectId.IsNone ) {
                return new DeleteProjectPayload( "Invalid project ID to be deleted" );
            }

            var retValue = EntityId.Default;
            var currentProject = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));
            var result = await currentProject.BindAsync( p => {
                retValue = p.EntityId;

                return user.BindAsync( u => mProjectProvider.DeleteProject( p, u ));
            } );

            return result.Match( _ => new DeleteProjectPayload( retValue ), e => new DeleteProjectPayload( e ));
        }
    }
}

