using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Platform;
using SquirrelsNest.Core.Transfer.Dto;

namespace SquirrelsNest.Core.Transfer.Export {
    internal class ExportManager : IExportManager {
        private readonly IProjectBuilder    mProjectBuilder;
        private readonly IIssueProvider     mIssueProvider;
        private readonly IFileWriter        mFileWriter;

        public ExportManager( IProjectBuilder projectBuilder, IIssueProvider issueProvider, IFileWriter fileWriter ) {
            mProjectBuilder = projectBuilder;
            mIssueProvider = issueProvider;
            mFileWriter = fileWriter;
        }

        private EitherAsync<Error, Unit> ExportEntities( TransferEntities entities, ExportParameters parameters ) {
            return mFileWriter.SaveAsync( parameters.ExportFilePath, entities );
        }

        private bool IssueFilter( SnIssue issue, ExportParameters parameters, IReadOnlyList<SnWorkflowState> states ) {
            if( parameters.IncludeCompletedIssues ) {
                return true;
            }

            var state = states.FirstOrDefault( s => s.EntityId.Equals( issue.WorkflowStateId ), SnWorkflowState.Default );

            return !state.Category.Equals( StateCategory.Completed ) &&
                   !state.Category.Equals( StateCategory.Terminal );
        }

        public async Task<Either<Error, Unit>> ExportProject( ExportParameters parameters ) {
            var project = await mProjectBuilder.BuildCompositeProject( parameters.Project );
            var issues = await mIssueProvider.GetIssues( parameters.Project );

            var transferEntity = project
                    .Map( p => new TransferEntities( p ))
                    .Map( te => te.With( te.CompositeProject.Components.Map( TrComponent.From )))
                    .Map( te => te.With( te.CompositeProject.IssueTypes.Map( TrIssueType.From )))
                    .Map( te => te.With( te.CompositeProject.Releases.Map( TrRelease.From )))
                    .Map( te => te.With( te.CompositeProject.WorkflowStates.Map( TrWorkflowState.From )));

            var entityWithIssues = 
                from issueList in issues
                from te in transferEntity
                    select te.With( issueList
                        .Where( issue => IssueFilter( issue, parameters, te.CompositeProject.WorkflowStates ))
                        .Map( TrIssue.From ));

            return await entityWithIssues.BindAsync( async entities => await ExportEntities( entities, parameters ));
        }
    }
}
