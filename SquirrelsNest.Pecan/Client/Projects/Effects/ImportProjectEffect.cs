using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class ImportProjectEffect : Effect<ImportProjectAction> {
        private readonly IDialogService     mDialogService;
        private readonly IDispatcher        mDispatcher;

        public ImportProjectEffect( IDialogService dialogService, IDispatcher dispatcher ) {
            mDialogService = dialogService;
            mDispatcher = dispatcher;
        }

        public override async Task HandleAsync( ImportProjectAction action, IDispatcher dispatcher ) {
            var importAction = new ImportProjectSubmit( new ImportProjectRequest());
            var parameters = new DialogParameters { { nameof( ImportProjectDialog.Request ), importAction } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<ImportProjectDialog>( "Import Project Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Canceled ) &&
               ( dialogResult.Data is ImportProjectSubmit request )) {
                mDispatcher.Dispatch( request );
            }
        }
    }
}
