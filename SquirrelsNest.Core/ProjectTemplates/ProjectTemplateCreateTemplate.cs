using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.ProjectTemplates {
    internal partial class ProjectTemplateManager {
        private IEnumerable<IssueTypeDescription> GetIssueTypes( SnProject forProject ) {
            return mIssueTypeProvider
                .GetIssues( forProject ).Result
                .Map( list => from i in list select IssueTypeDescription.From( i ))
                .IfLeft( new List<IssueTypeDescription>());
        }

        private IEnumerable<ComponentDescription> GetComponents( SnProject forProject ) {
            return mComponentProvider
                .GetComponents( forProject ).Result
                .Map( list => from c in list select ComponentDescription.From( c ))
                .IfLeft( new List<ComponentDescription>());
        }

        private IEnumerable<WorkflowStepDescription> GetWorkflowStates( SnProject forProject ) {
            return mStateProvider
                .GetStates( forProject ).Result
                .Map( list => from s in list select WorkflowStepDescription.From( s ))
                .IfLeft( new List<WorkflowStepDescription>());
        }

        public Either<Error, Unit> CreateTemplate( SnProject fromProject, TemplateParameters parameters ) {
            var template = new ProjectTemplate {
                TemplateName = parameters.TemplateName,
                TemplateDescription = parameters.TemplateDescription,
                Components = new List<ComponentDescription>( GetComponents( fromProject )),
                IssueTypes = new List<IssueTypeDescription>( GetIssueTypes( fromProject )),
                WorkflowSteps = new List<WorkflowStepDescription>( GetWorkflowStates( fromProject ))
            };

            return mSerializer.SaveTemplate( template, parameters.TemplateName );
        }
    }
}
