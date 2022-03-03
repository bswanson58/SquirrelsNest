using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.ProjectTemplates {
    internal partial class ProjectTemplateManager {

        private Either<Error, IEnumerable<SnComponent>> CreateComponents( IEnumerable<ComponentDescription> components, SnProject forProject ) {
            SnComponent CreateComponent( ComponentDescription fromDescription ) =>
                new SnComponent( fromDescription.Name )
                    .With( description: fromDescription.Description )
                    .For( forProject );

            Either<Error, SnComponent> AddComponent( ComponentDescription entityDescription ) =>
                mComponentProvider.AddComponent( CreateComponent( entityDescription )).Result;
                
            return ( from component in components select AddComponent( component )).Sequence();
        }

        private Either<Error, IEnumerable<SnIssueType>> CreateIssueTypes( IEnumerable<IssueTypeDescription> issueTypes, SnProject forProject ) {
            SnIssueType CreateIssueType( IssueTypeDescription fromDescription ) =>
                new SnIssueType( fromDescription.Name )
                    .With( description: fromDescription.Description )
                    .For( forProject );

            Either<Error, SnIssueType> AddIssueType( IssueTypeDescription entityDescription ) =>
                mIssueTypeProvider.AddIssue( CreateIssueType( entityDescription )).Result;

            return ( from issueType in issueTypes select AddIssueType( issueType )).Sequence();
        }

        private Either<Error, IEnumerable<SnWorkflowState>> CreateWorkflowSteps( IEnumerable<WorkflowStepDescription> states, SnProject forProject ) {
            SnWorkflowState CreateWorkflowState( WorkflowStepDescription fromDescription ) =>
                new SnWorkflowState( fromDescription.Name )
                    .With( description: fromDescription.Description, category: fromDescription.Category )
                    .For( forProject );

            Either<Error, SnWorkflowState> AddWorkflowState( WorkflowStepDescription entityDescription ) =>
                mStateProvider.AddState( CreateWorkflowState( entityDescription )).Result;

            return ( from state in states select AddWorkflowState( state )).Sequence();
        }

        private Either<Error, SnProject> CreateNewProject( ProjectParameters parameters ) {
            SnProject CreateSnProject() =>
                new SnProject( parameters.ProjectName, parameters.ProjectPrefix ).With( description: parameters.ProjectDescription );

            return mProjectProvider.AddProject( CreateSnProject()).Result;
        }

        public Either<Error, SnProject> CreateProject( ProjectTemplate template, ProjectParameters parameters ) {
            return 
                from project in CreateNewProject( parameters )
                from c in CreateComponents( template.Components, project )
                from i in CreateIssueTypes( template.IssueTypes, project )
                from w in CreateWorkflowSteps( template.WorkflowSteps, project )
                select project;
        }

    }
}
