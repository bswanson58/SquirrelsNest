using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class AddProjectEffect : Effect<AddProjectAction> {
        private readonly IDialogService mDialogService;

        public AddProjectEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( AddProjectAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters { { "project", new CreateProjectInput() } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<AddProjectDialog>( "New Project Parameter", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is CreateProjectInput projectInput )) {
                dispatcher.Dispatch( new AddProjectSubmitAction( projectInput ));
            }
        }
    }
}
