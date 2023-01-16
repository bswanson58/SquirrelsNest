using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Server.Features.Transfer.Dto;
using SquirrelsNest.Pecan.Server.Support;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Transfer {
    public interface IExportManager {
        Task<Stream> ExportProject( SnCompositeProject project, bool includeCompletedIssues );
    }

    public class ExportManager : IExportManager {
        private readonly IIssueProvider         mIssueProvider;
        private readonly IStreamWriter          mStreamWriter;
        private readonly ILogger<ExportManager> mLogger;

        public ExportManager( IIssueProvider issueProvider, IStreamWriter streamWriter, ILogger<ExportManager> logger ) {
            mIssueProvider = issueProvider;
            mStreamWriter = streamWriter;
            mLogger = logger;
        }

        private bool IssueFilter( SnIssue issue, bool includeCompletedIssues, IReadOnlyList<SnWorkflowState> states ) {
            if( includeCompletedIssues ) {
                return true;
            }

            var state = states.FirstOrDefault( s => s.EntityId.Equals( issue.WorkflowStateId ), SnWorkflowState.Default );

            return !state.Category.Equals( StateCategory.Completed ) &&
                   !state.Category.Equals( StateCategory.Terminal );
        }
        
        private async Task<Stream> StreamEntities( TransferEntities entities ) {
            var stream = new MemoryStream() as Stream;

            try {
                await mStreamWriter.SaveAsync( stream, entities );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );

                await stream.DisposeAsync();
                stream = Stream.Null;
            }

            stream.Seek( 0, SeekOrigin.Begin );

            return stream;
        }

        public Task<Stream> ExportProject( SnCompositeProject project, bool includeCompletedIssues ) {
            var transferEntities = new TransferEntities {
                Project = TrProject.From( project.Project )
            };

            transferEntities.Components.AddRange( project.Components.Select( TrComponent.From ));
            transferEntities.IssueTypes.AddRange( project.IssueTypes.Select( TrIssueType.From ));
            transferEntities.Releases.AddRange( project.Releases.Select( TrRelease.From ));
            transferEntities.WorkflowStates.AddRange( project.WorkflowStates.Select( TrWorkflowState.From ));
            transferEntities.Users.AddRange( project.Users.Select( TrUser.From ));

            var issues = mIssueProvider.GetAll( project.Project );

            transferEntities.Issues
                .AddRange( issues
                    .ToList()
                    .Where( i => IssueFilter( i, includeCompletedIssues, project.WorkflowStates ))
                    .Select( TrIssue.From ));

            return StreamEntities( transferEntities );
        }
    }
}
