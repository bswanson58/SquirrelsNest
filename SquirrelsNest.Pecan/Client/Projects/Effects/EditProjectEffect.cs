using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class EditProjectEffect : Effect<EditProjectAction> {
        private readonly IDialogService mDialogService;

        public EditProjectEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( EditProjectAction action, IDispatcher dispatcher ) {
            var editProjectRequest = new EditProjectRequest( action.Project );
            var parameters = new DialogParameters { { nameof( EditProjectDialog.Project ), editProjectRequest } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<EditProjectDialog>( "Project Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is EditProjectRequest request )) {
                dispatcher.Dispatch( new EditProjectSubmit( request ));
            }
        }
    }
}
