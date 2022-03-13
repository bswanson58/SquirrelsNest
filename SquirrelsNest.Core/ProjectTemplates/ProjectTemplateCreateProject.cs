using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.ProjectTemplates {
    internal partial class ProjectTemplateManager {

        private async Task<Either<Error, SnProject>> CreateComponents( IList<ComponentDescription> components, SnProject forProject ) {
            SnComponent CreateComponent( ComponentDescription fromDescription ) =>
                new SnComponent( fromDescription.Name )
                    .With( description: fromDescription.Description )
                    .For( forProject );

            foreach( var component in components ) {
                var retValue = await mComponentProvider.AddComponent( CreateComponent( component )).ConfigureAwait( false );

                if( retValue.IsLeft ) {
                    return retValue.Map( _ => forProject );
                }
            }

            return forProject;
        }

        private async Task<Either<Error, SnProject>> CreateIssueTypes( IEnumerable<IssueTypeDescription> issueTypes, SnProject forProject ) {
            SnIssueType CreateIssueType( IssueTypeDescription fromDescription ) =>
                new SnIssueType( fromDescription.Name )
                    .With( description: fromDescription.Description )
                    .For( forProject );

            foreach( var issueType in issueTypes ) {
                var retValue = await mIssueTypeProvider.AddIssue( CreateIssueType( issueType )).ConfigureAwait( false );

                if( retValue.IsLeft ) {
                    return retValue.Map( _ => forProject );
                }
            }

            return forProject;
        }

        private async Task<Either<Error, SnProject>> CreateWorkflowSteps( IEnumerable<WorkflowStepDescription> states, SnProject forProject ) {
            SnWorkflowState CreateWorkflowState( WorkflowStepDescription fromDescription ) =>
                new SnWorkflowState( fromDescription.Name )
                    .With( description: fromDescription.Description, category: fromDescription.Category )
                    .For( forProject );

            foreach( var state in states ) {
                var retValue = await mStateProvider.AddState( CreateWorkflowState( state )).ConfigureAwait( false );

                if( retValue.IsLeft ) {
                    return retValue.Map( _ => forProject );
                }
            }

            return forProject;
        }

        private Task<Either<Error, SnProject>> CreateNewProject( ProjectParameters parameters, SnUser forUser ) {
            SnProject CreateSnProject() =>
                new SnProject( parameters.ProjectName, parameters.ProjectPrefix ).With( description: parameters.ProjectDescription );

            return mProjectProvider.AddProject( CreateSnProject(), forUser );
        }

        public async Task<Either<Error, SnProject>> CreateProject( ProjectTemplate template, ProjectParameters parameters, SnUser forUser ) {
            var project = await CreateNewProject( parameters, forUser ).ConfigureAwait( false );

            return await project
                    .BindAsync( p => CreateComponents( template.Components, p ))
                    .BindAsync( p => CreateIssueTypes( template.IssueTypes, p ))
                    .BindAsync( p => CreateWorkflowSteps( template.WorkflowSteps, p ))
                    .ConfigureAwait( false );
        }
    }
}
