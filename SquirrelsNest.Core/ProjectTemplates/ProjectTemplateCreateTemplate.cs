using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.ProjectTemplates {
    internal partial class ProjectTemplateManager {
        private async Task<IEnumerable<IssueTypeDescription>> GetIssueTypes( SnProject forProject ) {
            return ( await mIssueTypeProvider.GetIssues( forProject ).ConfigureAwait( false ))
                .Map( list => from i in list select IssueTypeDescription.From( i ))
                .IfLeft( Enumerable.Empty<IssueTypeDescription>());
        }

        private async Task<IEnumerable<ComponentDescription>> GetComponents( SnProject forProject ) {
            return ( await  mComponentProvider.GetComponents( forProject ).ConfigureAwait( false ))
                .Map( list => from c in list select ComponentDescription.From( c ))
                .IfLeft( Enumerable.Empty<ComponentDescription>());
        }

        private async Task<IEnumerable<WorkflowStepDescription>> GetWorkflowStates( SnProject forProject ) {
            return ( await mStateProvider.GetStates( forProject ).ConfigureAwait( false ))
                .Map( list => from s in list select WorkflowStepDescription.From( s ))
                .IfLeft( Enumerable.Empty<WorkflowStepDescription>());
        }

        public async Task<Either<Error, Unit>> CreateTemplate( SnProject fromProject, TemplateParameters parameters ) {
            var template = new ProjectTemplate {
                TemplateName = parameters.TemplateName,
                TemplateDescription = parameters.TemplateDescription,
                Components = new List<ComponentDescription>( await GetComponents( fromProject ).ConfigureAwait( false )),
                IssueTypes = new List<IssueTypeDescription>( await GetIssueTypes( fromProject ).ConfigureAwait( false )),
                WorkflowSteps = new List<WorkflowStepDescription>( await GetWorkflowStates( fromProject ).ConfigureAwait( false ))
            };

            return mSerializer.SaveTemplate( template, parameters.TemplateName );
        }
    }
}
