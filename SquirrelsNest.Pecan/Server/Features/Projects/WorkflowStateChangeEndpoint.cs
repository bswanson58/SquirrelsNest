using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsNest.Pecan.Server.Database.DataProviders;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Projects {
    [Authorize]
    [Route(WorkflowStateChangeInput.Route)]
    public class WorkflowStateChangeEndpoint : EndpointBaseAsync
        .WithRequest<WorkflowStateChangeInput>
        .WithActionResult<WorkflowStateChangeResponse> {

        private readonly IWorkflowStateProvider                 mWorkflowStateProvider;
        private readonly IProjectProvider                       mProjectProvider;
        private readonly IValidator<WorkflowStateChangeInput>   mValidator;

        public WorkflowStateChangeEndpoint( IWorkflowStateProvider workflowStateProvider, IProjectProvider projectProvider,
                                            IValidator<WorkflowStateChangeInput> validator) {
            mWorkflowStateProvider = workflowStateProvider;
            mProjectProvider = projectProvider;
            mValidator = validator;
        }

        private async Task<bool> IsValidProject( string projectId ) {
            var project = await mProjectProvider.GetById( projectId );

            return project != null;
        }

        private async Task<SnWorkflowState> AddWorkflowState( SnWorkflowState workflowState ) {
            return await mWorkflowStateProvider.Create( workflowState );
        }

        private async Task<SnWorkflowState> UpdateWorkflowState( SnWorkflowState workflowState ) {
            var updatedWorkflowState = await mWorkflowStateProvider.Update( workflowState );

            if( updatedWorkflowState != null ) {
                return updatedWorkflowState;
            }

            throw new ApplicationException( "WorkflowState to update was not located." );
        }

        private async Task<SnWorkflowState> DeleteWorkflowState( SnWorkflowState workflowState ) {
             await mWorkflowStateProvider.Delete( workflowState );

             return workflowState;
        }

        [HttpPost]
        public override async Task<ActionResult<WorkflowStateChangeResponse>> HandleAsync( 
            [FromBody]WorkflowStateChangeInput request,
            CancellationToken cancellationToken = new ()) {

            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new WorkflowStateChangeResponse( validInput ));
                }

                if(!( await IsValidProject( request.WorkflowState.ProjectId ))) {
                    return Ok( new WorkflowStateChangeResponse( "Invalid project specified in WorkflowState input" ));
                }

                switch ( request.ChangeType ) {
                    case EntityChangeType.Add:
                        return Ok( new WorkflowStateChangeResponse( await AddWorkflowState( request.WorkflowState ), request.ChangeType ));

                    case EntityChangeType.Update:
                        return Ok( new WorkflowStateChangeResponse( await UpdateWorkflowState( request.WorkflowState ), request.ChangeType ));

                    case EntityChangeType.Delete:
                        return Ok( new WorkflowStateChangeResponse( await DeleteWorkflowState( request.WorkflowState ), request.ChangeType ));

                    default:
                        return Ok( new WorkflowStateChangeResponse( "Unknown change type requested" ));
                }

            }
            catch( Exception ex ) {
                return Ok( new WorkflowStateChangeResponse( ex ));
            }
        }
    }
}
