using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class ImportProjectSubmitEffect : Effect<ImportProjectSubmit> {
        private readonly IAuthenticatedHttpHandler          mHttpHandler;
        private readonly ILogger<ImportProjectSubmitEffect> mLogger;

        public ImportProjectSubmitEffect( IAuthenticatedHttpHandler httpHandler,
                                          ILogger<ImportProjectSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( ImportProjectSubmit action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Importing Project" ));

            try {
                if( action.File != null ) {
                    using var reader = new StreamReader( action.File.OpenReadStream());

                    action.Request.ImportContent = Convert.ToBase64String( Encoding.UTF8.GetBytes( await reader.ReadToEndAsync()));

                    var response = await mHttpHandler.Post<ImportProjectResponse>( ImportProjectRequest.Route, action.Request );

                    if(( response?.Project != null ) &&
                       ( response.Succeeded )) {
                        dispatcher.Dispatch( new ImportProjectSuccess( response.Project ));
                    }
                    else {
                        dispatcher.Dispatch( new ImportProjectFailure( response?.Message ?? "Received null response" ));
                    }
                }
                else {
                    dispatcher.Dispatch( new ImportProjectFailure( "No import file was specified" ));
                }
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );

                dispatcher.Dispatch( new AddProjectFailure( ex.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
