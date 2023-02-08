using System;
using System.Threading.Tasks;
using Blazor.DownloadFileFast.Interfaces;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class ExportProjectSubmitEffect : Effect<ExportProjectSubmit> {
        private readonly IAuthenticatedHttpHandler          mHttpHandler;
        private readonly IBlazorDownloadFileService         mFileService;
        private readonly ILogger<ExportProjectSubmitEffect> mLogger;

        public ExportProjectSubmitEffect( IAuthenticatedHttpHandler httpHandler, IBlazorDownloadFileService fileService, 
                                          ILogger<ExportProjectSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mFileService = fileService;
            mLogger = logger;
        }

        public override async Task HandleAsync( ExportProjectSubmit action, IDispatcher dispatcher ) {
            try {
                var request = new ExportProjectRequest( action.Project.EntityId, action.IncludeCompletedIssues );
                var response = await mHttpHandler.Post( ExportProjectRequest.Route, request );

                if( response?.IsSuccessStatusCode == true ) {
                    var today = DateTimeProvider.Instance.CurrentDate.ToShortDateString().Replace( "/", "-" );
                    var fileName = $"{action.Project.Name} exported on {today}.json";
                    var bytes = await response.Content.ReadAsByteArrayAsync();

                    await mFileService.DownloadFileAsync( fileName, bytes );
                }
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }
        }
    }
}
