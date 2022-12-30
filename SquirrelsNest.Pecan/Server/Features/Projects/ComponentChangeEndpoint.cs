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
    [Route(ComponentChangeInput.Route)]
    public class ComponentChangeEndpoint : EndpointBaseAsync
        .WithRequest<ComponentChangeInput>
        .WithActionResult<ComponentChangeResponse> {

        private readonly IComponentProvider                 mComponentProvider;
        private readonly IProjectProvider                   mProjectProvider;
        private readonly IValidator<ComponentChangeInput>   mValidator;

        public ComponentChangeEndpoint( IComponentProvider componentProvider, IProjectProvider projectProvider,
                                        IValidator<ComponentChangeInput> validator) {
            mComponentProvider = componentProvider;
            mProjectProvider = projectProvider;
            mValidator = validator;
        }

        private async Task<bool> IsValidProject( string projectId ) {
            var project = await mProjectProvider.GetById( projectId );

            return project != null;
        }

        private async Task<SnComponent> AddComponent( SnComponent component ) {
            return await mComponentProvider.Create( component );
        }

        private async Task<SnComponent> UpdateComponent( SnComponent component ) {
            var updatedComponent = await mComponentProvider.Update( component );

            if( updatedComponent != null ) {
                return updatedComponent;
            }

            throw new ApplicationException( "Component to update was not located." );
        }

        private async Task<SnComponent> DeleteComponent( SnComponent component ) {
             await mComponentProvider.Delete( component );

             return component;
        }

        [HttpPost]
        public override async Task<ActionResult<ComponentChangeResponse>> HandleAsync( 
            [FromBody]ComponentChangeInput request,
            CancellationToken cancellationToken = new ()) {

            try {
                var validInput = await mValidator.ValidateAsync( request, cancellationToken );

                if(!validInput.IsValid ) {
                    return Ok( new ComponentChangeResponse( validInput ));
                }

                if(!( await IsValidProject( request.Component.ProjectId ))) {
                    return Ok( new ComponentChangeResponse( "Invalid project specified in component" ));
                }

                switch ( request.ChangeType ) {
                    case EntityChangeType.Add:
                        return Ok( new ComponentChangeResponse( await AddComponent( request.Component ), request.ChangeType ));

                    case EntityChangeType.Update:
                        return Ok( new ComponentChangeResponse( await UpdateComponent( request.Component ), request.ChangeType ));

                    case EntityChangeType.Delete:
                        return Ok( new ComponentChangeResponse( await DeleteComponent( request.Component ), request.ChangeType ));

                    default:
                        return Ok( new ComponentChangeResponse( "Unknown change type requested" ));
                }

            }
            catch( Exception ex ) {
                return Ok( new ComponentChangeResponse( ex ));

            }
        }
    }
}
