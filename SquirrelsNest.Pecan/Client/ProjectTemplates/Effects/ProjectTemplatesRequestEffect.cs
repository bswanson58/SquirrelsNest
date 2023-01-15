using System;
using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.ProjectTemplates.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates;

namespace SquirrelsNest.Pecan.Client.ProjectTemplates.Effects {
    // ReSharper disable once UnusedType.Global
    public class ProjectTemplatesRequestEffect : Effect<RequestProjectTemplatesAction> {
        private readonly IAuthenticatedHttpHandler  mHttpHandler;
        private readonly IDispatcher                mDispatcher;

        public ProjectTemplatesRequestEffect( IAuthenticatedHttpHandler httpHandler, IDispatcher dispatcher ) {
            mHttpHandler = httpHandler;
            mDispatcher = dispatcher;
        }

        public override async Task HandleAsync( RequestProjectTemplatesAction action, IDispatcher dispatcher ) {
            try {
                var request = new GetProjectTemplatesRequest( new PageRequest( 1, 25 ));
                var response = await mHttpHandler.Post<GetProjectTemplatesResponse>( GetProjectTemplatesRequest.Route, request );

                if(( response?.Templates != null ) &&
                   ( response.Succeeded )) {
                    mDispatcher.Dispatch( new RequestProjectTemplatesSuccess( response.Templates ));
                }
                else {
                    mDispatcher.Dispatch( new RequestProjectTemplatesFailure( response?.Message ?? "Response message was null" ));
                }
            }
            catch( Exception ex ) {
                mDispatcher.Dispatch( new RequestProjectTemplatesFailure( ex.Message ));
            }
        }
    }
}
