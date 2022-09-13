using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.ProjectTemplates;
using SquirrelsNest.Service.Dto;
using SquirrelsNest.Service.Dto.Mutations;
using SquirrelsNest.Service.Support;

namespace SquirrelsNest.Service.Projects {
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class ProjectMutations : BaseGraphProvider {
        private readonly IProjectProvider           mProjectProvider;
        private readonly IProjectBuilder            mProjectBuilder;
        private readonly IProjectTemplateManager    mTemplateManager;

        public ProjectMutations( IUserProvider userProvider, IProjectProvider projectProvider, IProjectBuilder projectBuilder,
                                 IProjectTemplateManager templateManager, IHttpContextAccessor contextAccessor, IApplicationLog log ) :
            base( userProvider, contextAccessor, log ){
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mTemplateManager = templateManager;
        }

        private async Task<Either<Error, SnProject>> CreateProjectFromTemplate( AddProjectInput projectInput, 
                                                                                Either<Error, SnUser> user ) {
            var templateParameters = new ProjectParameters {
                ProjectName = projectInput.Title,
                ProjectDescription = projectInput.Description,
                ProjectPrefix = projectInput.IssuePrefix
            };
            var template = mTemplateManager.GetAvailableTemplates()
                .FirstOrDefault( t => t.TemplateName.Equals( projectInput.ProjectTemplate, StringComparison.InvariantCultureIgnoreCase ));

            if( template == null ) {
                return Error.New( $"Template named ${projectInput.ProjectTemplate} could not be found" );
            }

            return await user.BindAsync( u => mTemplateManager.CreateProject( template, templateParameters, u  ));
        }

        private async Task<Either<Error, SnProject>> CreateProject( AddProjectInput projectInput, 
                                                                    Either<Error, SnUser> user ) {
            var newProject = ( new SnProject( projectInput.Title, projectInput.IssuePrefix ))
                                .With( description: projectInput.Description );

            return await user.BindAsync( u => mProjectProvider.AddProject( newProject, u ));
        }

        [Authorize( Policy = PolicyNames.UserPolicy )]
        public async Task<AddProjectPayload> AddProject( AddProjectInput projectInput ) {
            var user = await GetUser();

            if( user.IsLeft ) {
                return new AddProjectPayload( "The user could not be determined" );
            }

            var addedProject = String.IsNullOrWhiteSpace( projectInput.ProjectTemplate ) ? 
                CreateProject( projectInput, user ) :
                CreateProjectFromTemplate( projectInput, user );
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

        public async Task<CreateTemplatePayload> CreateProjectTemplate( CreateTemplateInput templateInput ) {
            var templateParameters = new TemplateParameters {
                TemplateName = templateInput.Name, 
                TemplateDescription = templateInput.Description
            };
            var projectId = EntityId.For( templateInput.ProjectId );
            var project = await projectId.MapAsync( id => mProjectProvider.GetProject( id ));
            var result = await project.BindAsync( p => mTemplateManager.CreateTemplate( p, templateParameters ));

            return result.Match( _ => new CreateTemplatePayload(), e => new CreateTemplatePayload( e ));
        }
    }
}

