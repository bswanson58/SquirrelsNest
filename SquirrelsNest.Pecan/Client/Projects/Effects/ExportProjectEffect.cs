using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class ExportProjectEffect : Effect<ExportProjectAction> {
        private readonly IDialogService mDialogService;

        public ExportProjectEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( ExportProjectAction action, IDispatcher dispatcher ) {
            var dialogInput = new ExportProjectSubmit( action.Project, false );
            var parameters = new DialogParameters { { nameof( ExportProjectDialog.Request ), dialogInput } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<ExportProjectDialog>( "Export Project", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Canceled ) &&
               ( dialogResult.Data is ExportProjectSubmit request )) {
                dispatcher.Dispatch( new ExportProjectSubmit( request.Project, request.IncludeCompletedIssues ));
            }
        }
    }
}
