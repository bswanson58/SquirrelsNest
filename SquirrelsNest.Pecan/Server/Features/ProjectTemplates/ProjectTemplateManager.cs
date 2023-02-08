using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.ProjectTemplates {
    public class ProjectTemplateManager : IProjectTemplateManager {
        private const string        ProjectTemplatesDataType = "projectTemplate";

        private readonly IUserDataProvider                  mUserDataProvider;
        private readonly IProjectProvider                   mProjectProvider;
        private readonly IComponentProvider                 mComponentProvider;
        private readonly IIssueTypeProvider                 mIssueTypeProvider;
        private readonly IWorkflowStateProvider             mStateProvider;
        private readonly ILogger<ProjectTemplateManager>    mLogger;

        public ProjectTemplateManager( IUserDataProvider userDataProvider, IProjectProvider projectProvider,
                                       IComponentProvider componentProvider, IIssueTypeProvider issueTypeProvider,
                                       IWorkflowStateProvider stateProvider, ILogger<ProjectTemplateManager> logger ) {
            mUserDataProvider = userDataProvider;
            mProjectProvider = projectProvider;
            mComponentProvider = componentProvider;
            mIssueTypeProvider = issueTypeProvider;
            mStateProvider = stateProvider;
            mLogger = logger;
        }

        public async Task<IEnumerable<ProjectTemplate>> GetAvailableTemplates() {
            var templates = await mUserDataProvider.GetUserData( ProjectTemplatesDataType );

            return templates
                .Select( CreateTemplate )
                .Where( t => !String.IsNullOrWhiteSpace( t.TemplateName ));
        }

        private ProjectTemplate CreateTemplate( SnUserData fromData ) {
            try {
                return JsonSerializer.Deserialize<ProjectTemplate>( fromData.Data ) ?? new ProjectTemplate();
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );

                return new ProjectTemplate();
            }
        }

        public async Task<SnProject> CreateProject( ProjectTemplate fromTemplate, ProjectParameters parameters ) {
            var project = await mProjectProvider.Create( 
                new SnProject( parameters.ProjectName, parameters.ProjectDescription ).With( issuePrefix:parameters.IssuePrefix ));

            foreach ( var component in fromTemplate.Components ) {
                await mComponentProvider.Create(
                    new SnComponent( project ).With( name: component.Name, description: component.Description ));
            }

            foreach ( var issueType in fromTemplate.IssueTypes ) {
                await mIssueTypeProvider.Create( 
                    new SnIssueType( project ).With( name: issueType.Name, description: issueType.Description ));
            }

            foreach ( var state in fromTemplate.WorkflowStates ) {
                await mStateProvider.Create( 
                    new SnWorkflowState( project ).With( name: state.Name, description: state.Description ));
            }

            return project;
        }

        public async Task CreateTemplate( SnCompositeProject fromProject, TemplateParameters parameters ) {
            var template = new ProjectTemplate
                { TemplateName = parameters.TemplateName, TemplateDescription = parameters.TemplateDescription };

            foreach( var component in fromProject.Components ) {
                template.Components.Add( ComponentDescription.From( component ));
            }

            foreach ( var issueType in fromProject.IssueTypes ) {
                template.IssueTypes.Add( IssueTypeDescription.From( issueType ));
            }

            foreach ( var state in fromProject.WorkflowStates ) {
                template.WorkflowStates.Add( WorkflowStepDescription.From( state ));
            }

            var jsonData = JsonSerializer.Serialize( template );
            var userData = new SnUserData( SnUser.Default.EntityId, ProjectTemplatesDataType, jsonData );

            await mUserDataProvider.Create( userData );
        }
    }
}
